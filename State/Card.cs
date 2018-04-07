using System;

namespace State
{
    /// <summary>
    /// Кредитная карта.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Кредитные средства на карте.
        /// </summary>
        public decimal Credit { get; set; }

        /// <summary>
        /// Собственные средства на карте.
        /// </summary>
        public decimal Debit { get; set; }

        /// <summary>
        /// Все средства на карте.
        /// </summary>
        public decimal All => Credit + Debit;

        /// <summary>
        /// Состояние карты.
        /// </summary>
        public IState State { get; set; }

        /// <summary>
        /// Кредитный лимит на карте.
        /// </summary>
        public decimal CreditLimit { get; private set; }

        /// <summary>
        /// Создать новый экземпляр кредитной карты. 
        /// </summary>
        /// <param name="creditLimit"> Кредитный лимит. </param>
        public Card(decimal creditLimit)
        {
            // Проверяем входные данные на корректность.
            if(creditLimit <= 0)
            {
                throw new ArgumentException("Кредитный лимит должен быть больше нуля.", nameof(creditLimit));
            }

            // Устанавливаем значения.
            CreditLimit = creditLimit;
            Credit = creditLimit;
            State = new UsingOwnFunds();
            Debit = 0;
        }

        /// <summary>
        /// Пополнить карту.
        /// </summary>
        /// <param name="money"> Сумма пополнения. </param>
        public void Deposit(decimal money)
        {
            // Передаем управление пополнением текущему состоянию объекта.
            State.Deposit(this, money);
        }

        /// <summary>
        /// потратить деньги с карты.
        /// </summary>
        /// <param name="price"> Сумма покупки. </param>
        /// <returns> Успешность выполнения операции. </returns>
        public bool Spend(decimal price)
        {
            // Передаем управление расходом средств текущему состоянию объекта.
            return State.Spend(this, price);
        }

        /// <summary>
        /// Приведение объекта к строке.
        /// </summary>
        /// <returns> Состояние счета. </returns>
        public override string ToString()
        {
            return $"Состояние счета {All}, в том числе кредитные средства {Credit}, собственные средства {Debit}.";
        }
    }
}
