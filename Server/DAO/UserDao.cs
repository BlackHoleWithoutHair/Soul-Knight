using MySql.Data.MySqlClient;
using SoulKnightProtocol;
using System.Data;

public class UserDao
{
    private MySqlConnection conn;
    private string s = "database=soulknight; data source=localhost;user=root;password=123456;pooling=false;charset=utf8;port=3306";
    public UserDao()
    {
        ConnectDatabase();
    }
    private void ConnectDatabase()
    {
        try
        {
            conn = new MySqlConnection(s);
            conn.Open();
        }
        catch (Exception)
        {
            Console.WriteLine("连接数据库失败");
        }
    }
    public bool Register(MainPack pack)
    {
        string userName = pack.LoginPack.UserName;
        string password = pack.LoginPack.Password;
        string sql = "Insert into user(UserName,Password) values(@userName,@password)";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.Add(new MySqlParameter("@userName", SqlDbType.NVarChar));
        cmd.Parameters.Add(new MySqlParameter("@password", SqlDbType.NVarChar));
        cmd.Parameters["@userName"].Value = userName;
        cmd.Parameters["@password"].Value = password;
        try
        {
            cmd.ExecuteNonQuery();
            Console.WriteLine("数据库成功写入了一条数据");
        }
        catch (Exception)
        {
            Console.WriteLine("数据库插入数据失败");
            return false;
        }
        return true;
    }
    public bool Login(MainPack pack)
    {
        bool isFind = false;
        string sql = "Select * from user where UserName=@username and Password=@password";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.Add(new MySqlParameter("@username", SqlDbType.NVarChar));
        cmd.Parameters.Add(new MySqlParameter("@password", SqlDbType.NVarChar));
        cmd.Parameters["@username"].Value = pack.LoginPack.UserName;
        cmd.Parameters["@password"].Value = pack.LoginPack.Password;
        try
        {
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                isFind = true;
            }
            reader.Close();
        }
        catch (Exception)
        {
            Console.WriteLine("数据库查询数据失败");
        }

        return isFind;
    }
    public void CloseConn()
    {
        conn.Close();
    }
}