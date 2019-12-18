using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace DowntownRP.Utilities
{
    public class Generate
    {
        public static string CreateIBANBank()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        public async static Task<string> GenerateMatricula()
        {
            string matricula = CreateIBANBank();
            if (await CheckIfCompanyMatriculaExists(matricula)) matricula = CreateIBANBank();
            return matricula;
        }

        public async static Task<bool> CheckIfCompanyMatriculaExists(string numberplate)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * vehicles_companies WHERE numberplate = @numberplate";
                command.Parameters.AddWithValue("@numberplate", numberplate);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                return reader.HasRows;
            }
        }
    }
}
