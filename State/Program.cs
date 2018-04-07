using System;
using System.Text;

namespace State
{
    class Program
    {
        static void Main(string[] args)
        {
            // Используем эту команду, если Windows на английском 
            // и выводит вместо кириллицы ???????? ????? ????
            Console.OutputEncoding = Encoding.Unicode;

            // Создаем новую карту.
            var card = new Card(10);

            // Выполняем операции с картой.
            card.Deposit(1);    // 11
            card.Deposit(2);    // 13
            card.Spend(1);      // 12
            card.Spend(5);      // 7
            card.Deposit(7);    // 14
            card.Spend(10);     // 4
            card.Spend(1);      // 3
            card.Spend(5);      // 3
            card.Spend(2.5M);   // 0.5      
            card.Deposit(7);    // 7.5
            card.Spend(7);      // 0.5
            card.Deposit(0.1M); // 0.6
            card.Deposit(20);   // 20.6

            Console.ReadLine();
        }
    }
}
