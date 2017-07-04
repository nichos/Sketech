using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Sketech.Dals.Extensions
{
    public static class SqlDataReaderExtension
    {
        public static async Task<T> GetValueAsync<T>(this SqlDataReader reader, string columnName, T defaultValue = default(T))
        {
            try
            {
                var columnIndex = reader.GetOrdinal(columnName);
                if(await reader.IsDBNullAsync(columnIndex))
                {
                    return defaultValue;
                }

                return await reader.GetFieldValueAsync<T>(columnIndex);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
