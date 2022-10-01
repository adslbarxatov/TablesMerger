# TablesMerger v 2.0
>PCC: 00061BCD4EDD8836


A tool for data tables merging

Инструмент объединения таблиц данных


#

This tool allows you to merge data tables of different types into single file.
If you have these files:

Этот инструмент позволяет объединять таблицы данных различных типов в единый файл.
Если имеются следующие файлы:

```
  A B
1 1 1
2 4 5
3 6 7

  C D E
1 8 9 4
3 2 3 2
```

you can get these results (without fixing missing data and with it):

можно получить следующие результаты (без восстановления пропусков и с восстановлением пропусков):

```
  A B C D E
1 1 1 8 9 4
3 6 7 2 3 2

  A B C D E
1 1 1 8 9 4
2 4 5 0 0 0
3 6 7 2 3 2
```

(depends from selected settings)

(зависит от выбранных настроек)

&nbsp;



## Requirements / Требования

- Windows 7 or newer / или новее;
- [Microsoft .NET Framework 4.8](https://go.microsoft.com/fwlink/?linkid=2088631).

Interface languages / языки интерфейса: ru_ru, en_us.

&nbsp;



## [Development policy and EULA](https://adslbarxatov.github.io/ADP) / [Политика разработки и EULA](https://adslbarxatov.github.io/ADP/ru)

This Policy (ADP), its positions, conclusion, EULA and application methods
describes general rules that we follow in all of our development processes, released applications and implemented ideas.
***It must be acquainted by participants and users before using any of laboratory’s products.
By downloading them, you agree and accept this Policy!***

Данная Политика (ADP), её положения, заключение, EULA и способы применения
описывают общие правила, которым мы следуем во всех наших процессах разработки, вышедших в релиз приложениях
и реализованных идеях.
***Обязательна к ознакомлению для всех участников и пользователей перед использованием любого из продуктов лаборатории.
Загружая их, вы соглашаетесь и принимаете эту Политику!***
