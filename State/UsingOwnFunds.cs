using System;

namespace State
{
    /// <summary>
    /// Состояние счета, при котором используются собственные средства.
    /// </summary>
    public class UsingOwnFunds : IState
    {
        /// <summary>
        /// Пополнить счет.
        /// </summary>
        /// <param name="card"> Пополняемый счет. </param>
        /// <param name="money"> Сумма пополнения. </param>
        public void Deposit(Card card, decimal money)
        {
            // Проверяем входные аргументы на корректность.
            if(card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            if(money <= 0)
            {
                throw new ArgumentException("Вносимая сумма должна быть больше нуля.", nameof(money));
            }

            // Увеличиваем остаток собственных средств.
            card.Debit += money;

            Console.WriteLine($"Ваш счет пополнен на {money} рублей. {card.ToString()}");
        }

        /// <summary>
        /// Расходование со счета.
        /// </summary>
        /// <param name="card"> Счет списания. </param>
        /// <param name="price"> Стоимость покупки. </param>
        /// <returns> Успешность выполнения операции. </returns>
        public bool Spend(Card card, decimal price)
        {
            // Проверяем входные аргументы на корректность.
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            if (price <= 0)
            {
                throw new ArgumentException("Цена должна быть больше нуля.", nameof(price));
            }

            if(price <= card.Debit)
            {
                // Если сумма покупки меньше количества собственных средств,
                // то просто уменьшаем сумму собственных средств.
                card.Debit -= price;

                // Сообщаем пользователю.
                Console.WriteLine($"Выполнена операция за счет собственных средств на сумму {price}. {card.ToString()}");
                return true;
            }
            else if(price > card.All)
            {
                // Если сумма покупки больше, чем все средства на счету,
                // от отказываем в операции.
                Console.WriteLine($"Операция не выполнена. Недостаточно средств на счете. {card.ToString()}");
                return false;
            }
            else
            {
                // Иначе частично расходуем кредитные средства.
                // Вычисляем сумму необходимых кредитных средств.
                var overdraft = price - card.Debit;

                // Расходуем средства со счетов.
                card.Credit -= overdraft;
                card.Debit = 0;

                // Переводим карту в состояние расходования кредитных средств.
                card.State = new UsingCreditFunds();

                // Сообщаем пользователю.
                Console.WriteLine($"Выполнена операция за счет собственных и кредитных средств на сумму {price}. " +
                    $"Погасите задолженность в размере {overdraft} рублей.  {card.ToString()}");

                return true;
            }
        }
    }
}
