using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace WatchTable;

// % ***** Table class ***** % //
public class Table
{
    //: Fields and Properties
    public string Title       { get; set; }  /// Table Name
    public UInt32 CountColumn { get; set; }  /// Number of columns
    public string Corner      { get; set; }  /// Corner for table
    public string Separator   { get; set; }  /// Separator for table

    private Row        lColumnName  { get; set; }  /// Column name
    private List<int>  lColumnWidth { get; set; }  /// Сolumn width 
    private List<Row>  lRows        { get; set; }  /// Rows table

    //: Constructor's Table
    public Table() : this("Table Default") {}

    public Table(string nameT) {
        this.Title   = nameT;
        lColumnName  = new Row();
        lColumnWidth = new List<int>();
        lRows        = new List<Row>();

        Corner       = "\u002B";
        Separator    = "\u2010";
    }

    //: Adding a column to a table
    public void AddColumn(params (string, UInt32)[] lcolumns) {
        CountColumn += (UInt32)lcolumns.Length;
        for (int i = 0; i < lcolumns.Length; i++) {
            lColumnName.Add(lcolumns[i].Item1, (int)lcolumns[i].Item2);
            lColumnWidth.Add((int)lcolumns[i].Item2);
        }
    }
    
    //: Adding a row to a table
    public void AddRow(params string[] lrows) {
        if (lrows.Length != CountColumn)
            throw new Exception($"The count of parameters is not equal to the count of columns!");
        lRows.Add(new Row(lrows, lColumnWidth.ToArray()));
    }

    //: Override ToString
    public override string ToString() {
        
        // Copies
        var lrows   = new List<Row>();
        for (int i = 0; i < lRows.Count; i++)
            lrows.Add((Row)lRows[i].Clone());
        var lcolumn = (Row)lColumnName.Clone();

        // Table
        var table = new StringBuilder();

        // HLine
        string hline = Hline();

        // Title
        int sumWidth = lColumnWidth.Sum() + lColumnWidth.Count() - 1;
        if (Title.Length <= sumWidth) {

            int midWidth = (sumWidth - Title.Length) / 2;
            table.Append(hline + "|");
            table.Append(
                String.Join("", Enumerable.Repeat(" ", midWidth)) +
                Title + 
                String.Join("", Enumerable.Repeat(" ", (sumWidth - (midWidth + Title.Length))))
            );
            table.Append("|\n");
        
        } else {

            int repeat = Title.Length / sumWidth;
            string tempName = (string)Title.Clone();
            table.Append(hline);

            for (int i = 0; i < repeat; i++) {

                table.Append("|" + tempName.Substring(0, sumWidth));
                tempName = tempName.Remove(0, sumWidth);
                table.Append("|\n");

                if (tempName.Length <= sumWidth) {
                    int midWidth = (sumWidth - tempName.Length) / 2;
                    table.Append("|");
                    table.Append(
                        String.Join("", Enumerable.Repeat(" ", midWidth)) +
                        tempName + 
                        String.Join("", Enumerable.Repeat(" ", (sumWidth - (midWidth + tempName.Length))))
                    );
                    table.Append("|\n");
                    break;
                }
            }
        }

        // Columns name
        table.Append(hline);
        table.Append(lColumnName.ToString());
        table.Append(hline);

        // Rows
        for (int i = 0; i < lRows.Count; i++) {
            table.Append(lRows[i]);
            table.Append(hline);
        }

        // return copies
        lColumnName = lcolumn;
        lRows       = lrows;

        return table.ToString();
    }

    //: Method return hline
    private string Hline() {

        var hline = new StringBuilder();
        
        hline.Append(Corner);
        for (int i = 0; i < CountColumn; i++)
            hline.Append($"{String.Join("", Enumerable.Repeat(Separator, (int)lColumnWidth[i]))}" + Corner);
            
        return hline.Append("\n").ToString();;
    }

    //: Writing to a file
    public void WriteToFile(string path) {
        File.WriteAllText(path, this.ToString());
    }

    //: Writing to a CSV
    public void WriteToCSV(string path) {

        using (var writer = new StreamWriter(path)) {

            // Config for CSV
            var csvConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("ru-RU")) {
                HasHeaderRecord = false,
                Delimiter = ";"
            };

            using (var csv = new CsvWriter(writer, csvConfig)) {

                // Title
                for (int i = 0; i < CountColumn; i++)
                    csv.WriteField(lColumnName[i]);
                csv.NextRecord(); 

                // Rows
                for (int i = 0; i < lRows.Count(); i++) {
                    for (int j = 0; j < CountColumn; j++)
                        csv.WriteField(lRows[i][j].text);
                    csv.NextRecord();  
                }
            }
        }
    }
}


// % ***** Row class ***** %
internal class Row : ICloneable
{
    //: Fields and Properties
    private List<Cell> lcell;                 /// Text's

    public int Length { get; private set; }   /// Count array

    //: Constructor's
    public Row() {
        lcell  = new List<Cell>();
    }
    
    public Row(UInt32 _count) { 
        this.Length = (int)_count;
        this.lcell  = new List<Cell>();
    }

    public Row(string[] _texts, int[] _width) {
        this.Length = _width.Length;
        this.lcell  = new List<Cell>();
        for (int i = 0; i < this.Length; i++)
            lcell.Add(new Cell(_texts[i], _width[i]));
    }

    //: Indexer
    public Cell this[int i] {
        get => lcell[i];
        set => lcell[i] = value;
    }

    //: Adding string in list
    public void Add(string _text, int _width) {
        lcell.Add(new Cell(_text, _width));
        Length++;
    }


    //: Method ToString
    public override string ToString() {

        var row = new StringBuilder();
        
        int repeat = GetRow();
        for (int i = 0; i < repeat; i++) {
            row.Append("|");
            for (int j = 0; j < Length; j++) {
                row.Append(lcell[j].ToString());
                row.Append("|");
            }
            row.Append("\n");
        }

        return row.ToString();
    }

    //: Method return count row
    private int GetRow() {
        int count = 0;
        for (int i = 0; i < Length; i++) {
            double rows = Math.Ceiling(lcell[i].text.Length / (double)lcell[i].width);
            if (rows > count)
                count = (int)rows;
        }
        return count;
    }

    //: Method ICloneable
    public object Clone() {
        Row row = new Row();
        for (int i = 0; i < Length; i++)
            row.Add(lcell[i].text, lcell[i].width);
        return row;
    }
}

// % ***** Cell class ***** % //
internal class Cell : ICloneable
{
    //: Fields and Properties
    public string text;            /// Cell text
    public int    width;           /// Cell width
    public bool   IsEmpty;         /// Check the text for empty

    //: Constructor's
    public Cell(string _text, int _width) {
        this.text  = (string)_text.Clone();
        this.width = _width;
        IsEmpty    = false;
    }

    //: Method ToString
    public override string ToString() {

        var cell = new StringBuilder();

        if (IsEmpty || text == "_") {
            cell.Append(Tabs(0, width));
            return cell.ToString();
        }

        if (text.Length <= width) {
            cell.Append(text + Tabs(text.Length, width));
            IsEmpty = true;
            return cell.ToString();
        }

        cell.Append(text.Substring(0, width));
        text = text.Remove(0, width);

        return cell.ToString();
    }
    
    //: Method adding spaces
    private string Tabs(int _fill, int _need) {
        return String.Join("", Enumerable.Repeat(" ", (_need - _fill)));
    }

    //: Method ICloneable
    public object Clone() { return new Cell(text , width); }
}
