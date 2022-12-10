<p align="center">
  <img height="200" src="img/logo.png"/> 
</p>

<p align="center">
  <a href="https://en.wikipedia.org/wiki/C_Sharp_(programming_language)">
    <img src="https://img.shields.io/badge/C Sharp-11-blue.svg?cacheSeconds=2592000" alt="language"/>
  </a>	
   <a href="https://github.com/desmond60/WatchTable/blob/main/LICENSE.md">
    <img src="https://img.shields.io/badge/License-MIT-yellowgreen.svg" alt="license"/>
  </a>
  <a href="https://ru.wikipedia.org/wiki/.NET">
    <img src="https://img.shields.io/badge/.NET-7.0-important.svg?cacheSeconds=2592000" alt="standart"/>
  </a>
  <img src="https://img.shields.io/badge/version-1.0-green.svg?cacheSeconds=2592000" alt="version"/>
</p>

***

## Содержание <a name="content"></a>
* [Добавление библиотеки в приложение](#add-project)
    * [Первый способ](#first-way)
    * [Второй способ](#second-way)
    * [Третий способ](#third-way)
* [Создание таблицы](#create-table)
* [Запись таблицы в файл TXT](#write-txt)
* [Запись таблицы в файл CSV](#write-csv)
* [Изменение стиля таблицы](#edit-table)

***

## Добавление библиотеки в проект <a name="add-project"></a>

Добавить библиотеку в проект С# можно двумя способами. 

В первом способе вы можете редактировать библиотеку под свой вкус.

Во втором способе такой возможности не будет.

### Первый способ <a name="first-way"></a>
Добавление проекта библиотеки в ваше приложение.

Отсюда https://github.com/desmond60/WatchTable/releases/tag/v1.0 скачиваем `Source code` или клонируем репозиторий https://github.com/desmond60/WatchTable.git. Путь к проекту библиотеки у меня вглядит следующим образом: `"D:\Program Files\Libraries\WatchTable"`

Далее создадим консольное приложение.
```PS
dotnet new console -o TestingLib
```

Добавим в созданное консольное приложение ссылку на проект библиотеки.
```PS
dotnet add "TestingLib/TestingLib.csproj" reference "D:/Program Files/Libraries/WatchTable/WatchTable.csproj"
```

В `Program.cs` добавим:
```C#
using WatchTable;
```

### Второй способ <a name="second-way"></a>
Добавление библиотеки (dll) в ваше приложение.

Отсюда https://github.com/desmond60/WatchTable/releases/tag/v1.0 скачиваем `WatchTable.zip`. И кладем dll в проект, например `"lib/WatchTable.dll"`

В файл проекта (.csproj) добавляем следующее:
```XML
  <ItemGroup>
    <Reference Include="Table">
      <HintPath>lib\WatchTable.dll</HintPath>
    </Reference>
  </ItemGroup>
```

В `Program.cs` добавим:
```C#
using WatchTable;
```

### Третий способ <a name="third-way"></a>
Добавление библиотеки через NuGet Gallery.

Здесь можно посмотреть команду https://github.com/desmond60/WatchTable/packages/1733308

***

## Создание таблицы <a name="create-table"></a>

```C#
// Создание
Table table = new Table("MyTable");

// Добавление названия и ширины столбца
table.AddColumn(
    ("Number", 6),
    ("Variable", 15),
    ("Value", 10)
);

// Добавление строк
float Variable = 1e-15f;
table.AddRow("1", "X", "10");
table.AddRow("1", "Y", "-5");
table.AddRow("1", "Variable", Variable.ToString("E2"));

// Вывод
System.Console.WriteLine(table.ToString());
```

```
+------+---------------+----------+
|             MyTable             |
+------+---------------+----------+
|Number|Variable       |Value     |
+------+---------------+----------+
|1     |X              |10        |
+------+---------------+----------+
|1     |Y              |-5        |
+------+---------------+----------+
|1     |Variable       |1,00E-015 |
+------+---------------+----------+
```

***

## Запись таблицы в файл txt <a name="write-txt"></a>

Чтобы записать табличку в текстовый (.txt) файл, используйте функцию:

```C#
table.WriteToFile(@"table.txt");
```

***

## Запись таблицы в файл CSV <a name="write-csv"></a>

Чтобы записать табличку в файл (.csv), нужно установить `Nuget-пакет CsvHelper` и использовать функцию:

```C#
table.WriteToCSV(@"table.csv");
```

***

## Изменение стиля таблицы <a name="edit-table"></a>

Возможно понадобиться изменить кодировку консоли для полноценного вывода.

```C#
Console.OutputEncoding = System.Text.Encoding.UTF8;
```

Чтобы изменить таблицу, используйте свойства:

```C#
table.Corner = "\u2665";
table.Separator = "\u2504";
```

```
♥┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄♥
|             MyTable             |
♥┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄♥
|Number|Variable       |Value     |
♥┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄♥
|1     |X              |10        |
♥┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄♥
|1     |Y              |-5        |
♥┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄♥
|1     |Variable       |1,00E-015 |
♥┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄♥┄┄┄┄┄┄┄┄┄┄♥
```

### [Вернуться к содержимому](#content)
