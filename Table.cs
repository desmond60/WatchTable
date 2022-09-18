namespace WatchTable;

public class Table
{
    //* Поля класса
    public string Name        { get; set; }                         /// Имя таблицы
    public UInt16 CountColumn { get; private set; }                 /// Количество столбцов в таблице
    private List<(string, UInt16)> lTitleSizeColumn { get; init; }  /// Лист с размером и названием столбца  


    //* ----------------------------------------------- *//


    // ***** Конструкторы Table ***** //
    public Table() : this("Table Default", 1) {}

    public Table(string nameT) : this(nameT, 1) {}

    public Table(UInt16 countC) : this("Table Default", countC) {}

    public Table(string nameT, UInt16 countC, List<(string, UInt16)> lTSColumn) {
        this.Name        = nameT;
        this.CountColumn = countC;
        lTitleSizeColumn = lTSColumn;
    }
    //* ----------------------------------------------- *//




}
