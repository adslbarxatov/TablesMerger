# TablesMerger v 1.4h

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

you can get these results:

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

можно получить следующие результаты:

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

#

We've formalized our [Applications development policy (ADP)](https://vk.com/@rdaaow_fupl-adp).
We're strongly recommend reading it before using our products.

Мы формализовали нашу [Политику разработки приложений (ADP)](https://vk.com/@rdaaow_fupl-adp).
Настоятельно рекомендуем ознакомиться с ней перед использованием наших продуктов.

#

Needs Windows XP and newer, Framework 4.0 and newer. Interface languages: ru_ru, en_us

Требуется ОС Windows XP и новее, Framework 4.0 и новее. Языки интерфейса: ru_ru, en_us
