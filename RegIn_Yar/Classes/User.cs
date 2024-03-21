using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace RegIn_Yar.Classes
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public byte[] Image = new byte[0];
        public DateTime DateUpdate { get; set; }
        public DateTime DateCreate { get; set; }
        public CorrectLogin HandlerCorrectLogin;
        public InCorrectLogin HandlerInCorrectLogin;
        public delegate void CorrectLogin();
        public delegate void InCorrectLogin();
        public void GetUserLogin(string Login)
        {
            this.Id = -1;
            this.Login = String.Empty;
            this.Password = string.Empty;
            this.Name = String.Empty;
            this.Image = new byte[0];
            MySqlConnection mySqlConnection = WorkingDB.OpenConnection();
            if (WorkingDB.OpenConnection(mySqlConnection))
            {
                MySqlDataReader userQuary = WorkingDB.Quary($"Select * FROM `users` WHERE `Login`='{Login}'", mySqlConnection);
                if (userQuary.HasRows)
                {
                    userQuary.Read();
                    this.Id = userQuary.GetInt32(0);
                    this.Login = userQuary.GetString(1);
                    this.Password = userQuary.GetString(2);
                    this.Name = userQuary.GetString(3);
                    if (!userQuary.IsDBNull(4))
                    {
                        this.Image = new byte[64 * 1024];
                        userQuary.GetBytes(4, 0, Image, 0, Image.Length);
                    }
                    this.DateUpdate = userQuary.GetDateTime(5);
                    this.DateUpdate = userQuary.GetDateTime(6); //DateCreate!!!!!!
                    HandlerCorrectLogin.Invoke();
                }
                else
                    HandlerCorrectLogin.Invoke();
            }
            else
                HandlerCorrectLogin.Invoke();
            WorkingDB.CloseConnection(mySqlConnection);
        }
        public void SetUser()
        {
            MySqlConnection mySqlConnection = WorkingDB.OpenConnection();
            if (WorkingDB.OpenConnection(mySqlConnection))
            {
                MySqlCommand mySqlCommand = new MySqlCommand("INSERT INTO `users`(`Login`,`Password`,`Name`,`Image`,`DateUpdate`,`DateCreate`) VALUES (@Login,@Password,@Name,@Image,@DateUpdate,@DateCreate)", mySqlConnection);
                mySqlCommand.Parameters.AddWithValue("@Login", this.Login);
                mySqlCommand.Parameters.AddWithValue("@Password", this.Password);
                mySqlCommand.Parameters.AddWithValue("@Name", this.Name);
                mySqlCommand.Parameters.AddWithValue("@Image", this.Image);
                mySqlCommand.Parameters.AddWithValue("@DateUpdate", this.DateUpdate);
                mySqlCommand.Parameters.AddWithValue("@DateCreate", this.DateCreate);
                mySqlCommand.ExecuteNonQuery();
            }
            WorkingDB.CloseConnection(mySqlConnection);
        }
        public void CreateNewPassword()
        {
            if(Login != String.Empty)
            {
                Password = GeneratePass();
                MySqlConnection mySqlConnection = WorkingDB.OpenConnection();
                if (WorkingDB.OpenConnection(mySqlConnection)){
                    WorkingDB.Quary($"UPDATE `users` SET `Password`='{this.Password}' WHERE `Login`='{this.Login}'", mySqlConnection);
                }
                WorkingDB.CloseConnection(mySqlConnection);
                SendMail.SendMessage($"Your accoun password has been changed.\n New password:{this.Password}", this.Login);
            }
        }
        public string GeneratePass()
        {
            List<Char> NewPassword = new List<char>();
            Random rnd = new Random();
            char[] ArrNumbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] ArrSymbols = { '|', '-', '_', '!', '@', '#', '$', '%', '=', '+' };
            char[] ArrUppercase = { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
            for(int i = 0; i < 1; i++)
            {
                NewPassword.Add(ArrNumbers[rnd.Next(0, ArrNumbers.Length)]);
            }
            for(int i = 0; i < 1; i++)
            {
                NewPassword.Add(ArrSymbols[rnd.Next(0, ArrSymbols.Length)]);
            }
            for(int i=0; i < 2; i++)
            {
                NewPassword.Add(char.ToUpper(ArrUppercase[rnd.Next(0, ArrUppercase.Length)]));
            }
            for(int i = 0; i < 6; i++)
            {
                NewPassword.Add(ArrUppercase[rnd.Next(0, ArrUppercase.Length)]);
            }
            for(int i = 0; i < NewPassword.Count; i++)
            {
                int RandomSymbol = rnd.Next(0, NewPassword.Count);
                char Symbol = NewPassword[RandomSymbol];
                NewPassword[RandomSymbol] = NewPassword[i];
                NewPassword[i] = Symbol;
            }
            string NPassword = "";
            for (int i = 0; i < NewPassword.Count; i++)
                NPassword += NewPassword[i];
            return NPassword;
        }
    }
}
