namespace MiniORMTest
{
    using MiniORMTest.Data;
    public class StartUp
    {
        public static void Main(string[] args)
        {
            const string connectionString =
                "Server=DESKTOP-FJ4UOL0\\SQLEXPRESS;Database=MiniORM;Integrated Security=True";

            var context = new MiniORMDbContext(connectionString);


            
        }
    }
}
