﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс описывает главную форму програмы
	/// </summary>
	public partial class TablesMergerForm: Form
		{
		// Переменные
		private SupportedLanguages al = Localization.CurrentLanguage;

		// Исходные таблицы
		private List<List<List<double>>> dataTables = new List<List<List<double>>> ();
		private List<List<string>> columnNames = new List<List<string>> ();
		private List<uint> abscissasColumnsNumbers = new List<uint> ();

		private List<SourceTableRow> dataRows = new List<SourceTableRow> ();

		// Собранная таблица
		private List<List<double>> mergedTable = new List<List<double>> ();
		private List<string> mergedColumnNames = new List<string> ();

		// Флаги обработки
		private int mergeType;
		private bool success = false;

		/// <summary>
		/// Конструктор. Запускает интерфейс мерджера
		/// </summary>
		public TablesMergerForm ()
			{
			// Инициализация окна
			InitializeComponent ();

			LanguageCombo.Items.AddRange (Localization.LanguagesNames);
			try
				{
				LanguageCombo.SelectedIndex = (int)al;
				}
			catch
				{
				LanguageCombo.SelectedIndex = 0;
				}

			// Настройка контролов
			OFDialog.Multiselect = true;
			SFDialog.FilterIndex = 3;

			ProcessingResults.Items.Add (ProgramDescription.AssemblyTitle + Localization.GetText ("TablesMerger_LaunchedAt", al) +
				DateTime.Now.ToString ("dd.MM.yyyy HH:mm"));
			ProcessingResults.SelectedIndex = ProcessingResults.Items.Count - 1;

			this.Text = ProgramDescription.AssemblyTitle;
			}

		// Локализация интерфейса
		private void LanguageCombo_SelectedIndexChanged (object sender, EventArgs e)
			{
			// Сохранение
			Localization.CurrentLanguage = al = (SupportedLanguages)LanguageCombo.SelectedIndex;

			// Локализация
			OFDialog.Filter = Localization.GetText ("TablesMergerForm_SFDialog_F", al);
			OFDialog.Title = Localization.GetControlText ("GeomagDataDrawerForm", "OFDialog", al);
			SFDialog.Filter = string.Format (Localization.GetControlText ("GeomagDataDrawerForm", "SFDialog_F", al),
				"Geomag data drawer", ProgramDescription.AppDataExtension);
			SFDialog.Title = Localization.GetControlText ("GeomagDataDrawerForm", "SFDialog", al);

			while (MergeType.Items.Count < 2)
				{
				MergeType.Items.Add ("");
				MergeType.SelectedIndex = 0;
				}
			for (int i = 0; i < MergeType.Items.Count; i++)
				MergeType.Items[i] = Localization.GetText ("TablesMergerForm_MergeType" + i.ToString (), al);

			Localization.SetControlsText (this, al);
			BExit.Text = Localization.GetText ("MFile_MExit", al);
			}

		// Добавление файлов в обработку
		private void AddFiles_Click (object sender, EventArgs e)
			{
			OFDialog.ShowDialog ();
			}

		private void OFDialog_FileOk (object sender, CancelEventArgs e)
			{
			// Запрос параметров обработки файлов
			UnknownFileParametersSelector ufps = new UnknownFileParametersSelector (2, al, true);
			if (ufps.Cancelled)
				return;

			ColumnsNamesSelector cns = new ColumnsNamesSelector (0, al);
			if (cns.Cancelled)
				return;

			// Для каждого файла
			for (int i = 0; i < OFDialog.FileNames.Length; i++)
				{
				// Формирование таблицы данных
				DiagramData dd = new DiagramData (OFDialog.FileNames[i], ufps.DataColumnsCount, cns.SkippedRowsCount);
				if (dd.InitResult != DiagramDataInitResults.Ok)
					{
					ProcessingResults.Items.Add (string.Format (Localization.GetText ("FileAddError", al),
						Path.GetFileName (OFDialog.FileNames[i]),
						DiagramDataInitResultsMessage.ErrorMessage (dd.InitResult, al)));
					continue;
					}

				// Добавление в списки
				dataTables.Add (dd.GetData ());
				columnNames.Add (new List<string> ());
				for (uint c = 0; c < dd.DataColumnsCount; c++)
					{
					columnNames[columnNames.Count - 1].Add (dd.GetDataColumnName (c));
					}
				abscissasColumnsNumbers.Add (ufps.AbscissasColumn);
				FileNamesList.Items.Add (OFDialog.FileNames[i]);
				ProcessingResults.Items.Add (string.Format (Localization.GetText ("FileAddCompleted", al),
					Path.GetFileName (OFDialog.FileNames[i]),
					dataTables[dataTables.Count - 1].Count,
					dataTables[dataTables.Count - 1][0].Count));
				}

			// Завершено
			ProcessingResults.SelectedIndex = ProcessingResults.Items.Count - 1;
			}

		// Сброс списка файлов
		private void ClearFiles_Click (object sender, EventArgs e)
			{
			FileNamesList.Items.Clear ();
			dataTables.Clear ();
			columnNames.Clear ();
			abscissasColumnsNumbers.Clear ();

			ProcessingResults.Items.Add (Localization.GetText ("FilesListReset", al));
			ProcessingResults.SelectedIndex = ProcessingResults.Items.Count - 1;
			}

		// Запуск обработки
		private void BeginProcessing_Click (object sender, EventArgs e)
			{
			// Контроль
			if (dataTables.Count < 2)
				{
				MessageBox.Show (Localization.GetText ("NotEnoughFilesToMerge", al), ProgramDescription.AssemblyTitle,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
				}

			// Подготовка
			AddFiles.Enabled = ClearFiles.Enabled = MergeType.Enabled = BeginProcessing.Enabled =
				SaveResult.Enabled = BExit.Enabled = success = false;
			dataRows.Clear ();
			mergedTable.Clear ();
			mergedColumnNames.Clear ();
			mergeType = MergeType.SelectedIndex;

			// Запуск 
			HardWorkExecutor hwe = new HardWorkExecutor (ExecuteMerge);

			// Завершено
			AddFiles.Enabled = ClearFiles.Enabled = MergeType.Enabled = BeginProcessing.Enabled =
				BExit.Enabled = true;
			if (success)
				{
				ProcessingResults.Items.Add (string.Format (Localization.GetText ("TablesMergeCompleted", al),
					mergedTable.Count, mergedTable[0].Count));
				ProcessingResults.SelectedIndex = ProcessingResults.Items.Count - 1;
				SaveResult.Enabled = true;
				}
			}

		// Процесс, выполняющий объединение
		private void ExecuteMerge (object sender, DoWorkEventArgs e)
			{
			// Перегонка данных
			mergedColumnNames.Add ("x");

			for (int f = 0; f < dataTables.Count; f++)
				{
				// Сборка строк
				for (int r = 0; r < dataTables[f][(int)abscissasColumnsNumbers[f]].Count; r++)
					{
					// Возврат прогресса
					((BackgroundWorker)sender).ReportProgress ((int)(r * HardWorkExecutor.ProgressBarSize /
						dataTables[f][(int)abscissasColumnsNumbers[f]].Count),
						string.Format (Localization.GetText ("TablesAssembling", al), f + 1, r));

					// Создание строки
					dataRows.Add (new SourceTableRow ((uint)f, dataTables[f][(int)abscissasColumnsNumbers[f]][r]));

					for (int c = 0; c < dataTables[f].Count; c++)
						{
						// Пропуск столбца абсцисс
						if (c == abscissasColumnsNumbers[f])
							continue;

						// Добавление ординат
						dataRows[dataRows.Count - 1].AddOrdinate (dataTables[f][c][r]);

						// Завершение работы, если получено требование от диалога
						if (((BackgroundWorker)sender).CancellationPending)
							{
							e.Cancel = true;
							return;
							}
						}
					}

				// Сборка имён столбцов
				for (int c = 0; c < columnNames[f].Count; c++)
					{
					if (c != (int)abscissasColumnsNumbers[f])
						mergedColumnNames.Add (columnNames[f][c]);
					}
				}

			// Сортировка
			((BackgroundWorker)sender).ReportProgress ((int)HardWorkExecutor.ProgressBarSize,
				Localization.GetText ("TablesSorting", al));
			dataRows.Sort ();

			// Сборка итоговой таблицы
			double currentAbscissa = double.NaN;
			for (int r = 0; r < dataRows.Count; r++)
				{
				// Возврат прогресса
				((BackgroundWorker)sender).ReportProgress ((int)(r * HardWorkExecutor.ProgressBarSize / dataRows.Count),
					string.Format (Localization.GetText ("TablesMerging", al), r));

				// Добавление строк в таблицу
				if (currentAbscissa != dataRows[r].X)
					{
					currentAbscissa = dataRows[r].X;
					mergedTable.Add (new List<double> ());
					mergedTable[mergedTable.Count - 1].Add (dataRows[r].X);
					}

				// Сборка строк
				for (uint t = 0; t < dataTables.Count; t++)
					{
					// i может отличаться от r в случае пропусков строк
					int i = dataRows.IndexOf (new SourceTableRow (t, currentAbscissa));

					// Требуется дозаполнение
					if (i < 0)
						{
						switch (mergeType)
							{
							// Удаление неполных строк
							case 0:
								// Следующая схема вызовет выход из цикла обработки строк с данной ординатой
								mergedTable.RemoveAt (mergedTable.Count - 1);
								t = (uint)dataTables.Count - 1;
								break;

							// Заполнение нулями
							case 1:
								for (int c = 1; c < dataTables[(int)t].Count; c++)
									{
									mergedTable[mergedTable.Count - 1].Add (0);
									}
								break;
							}
						}

					// Объект найден
					else
						{
						mergedTable[mergedTable.Count - 1].AddRange (dataRows[i].Y);
						}
					}

				// Пропуск возможных дублей строк
				r = dataRows.FindLastIndex (x => x.X == currentAbscissa);
				}

			// Финальный контроль
			if (mergedTable.Count < 2)
				{
				MessageBox.Show (Localization.GetText ("TablesMergeError", al),
					ProgramDescription.AssemblyTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
				}

			// Завершено
			e.Result = null;
			success = true;
			}

		// Выход из программы
		private void BExit_Click (object sender, EventArgs e)
			{
			this.Close ();
			}

		private void TableMergerForm_FormClosing (object sender, FormClosingEventArgs e)
			{
			e.Cancel = MessageBox.Show (Localization.GetText ("TablesMerger_Exit", al),
				ProgramDescription.AssemblyTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes;
			}

		// Сохранение таблицы
		private void SaveResult_Click (object sender, EventArgs e)
			{
			SFDialog.ShowDialog ();
			}

		private void SFDialog_FileOk (object sender, CancelEventArgs e)
			{
			DiagramData dd = new DiagramData (mergedTable, mergedColumnNames);
			if (dd.SaveDataFile (SFDialog.FileName, (DataOutputTypes)SFDialog.FilterIndex, true) != 0)
				{
				MessageBox.Show (Localization.GetText ("DataFileSaveError", al), ProgramDescription.AssemblyTitle,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			else
				{
				ProcessingResults.Items.Add (Localization.GetText ("TablesMerger_FileSaved", al));
				ProcessingResults.SelectedIndex = ProcessingResults.Items.Count - 1;
				}
			}

		// Запрос справки о программе
		private void TablesMergerForm_HelpButtonClicked (object sender, CancelEventArgs e)
			{
			// Отмена обработки события вызова справки
			e.Cancel = true;

			// Отображение
			ProgramDescription.ShowAbout (false);
			}
		}
	}
