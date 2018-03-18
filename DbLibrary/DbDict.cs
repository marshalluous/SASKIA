using System.Data.SQLite;

namespace XYZLib
{
    public class XYZFile
    {
        SQLiteConnection bla = new SQLiteConnection("Data Source=:memory:");
        bla.Open();
    }
}
