# TablesMerger v 1.6
>PCC: 0005F415D6BCDD34


A tool for data tables merging

Инструмент объединения таблиц данных


#

This tool allows you to merge data tables of different types into single file.
If you have these files:

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



#

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

(зависит от выбранных настроек)

&nbsp;



## Requirements / Требования

Needs Windows XP and newer, Framework 4.0 and newer. Interface languages: ru_ru, en_us

Требуется ОС Windows XP и новее, Framework 4.0 и новее. Языки интерфейса: ru_ru, en_us

&nbsp;



## Development policy and EULA / Политика разработки и EULA

This [Policy (ADP)](https://vk.com/@rd_aaow_fdl-adp), its positions, conclusion, EULA and application methods
describes general rules that we follow in all of our development processes, released applications and implemented
ideas.
**It must be acquainted by participants and users before using any of laboratory's products.
By downloading them, you agree to this Policy**

#

Данная [Политика (ADP)](https://vk.com/@rd_aaow_fdl-adp), её положения, заключение, EULA и способы применения
описывают общие правила, которым мы следуем во всех наших процессах разработки, вышедших в релиз приложениях
и реализованных идеях.
**Обязательна к ознакомлению всем участникам и пользователям перед использованием любого из продуктов лаборатории.
Загружая их, вы соглашаетесь с этой Политикой**
