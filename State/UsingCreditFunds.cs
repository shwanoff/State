using System;

namespace State
{
    // Состояние счета, при котором используются кредитные средства.
    public class UsingCreditFunds : IState
    {
        /// <summary>
        /// Пополнить счет.
        /// </summary>
        /// <param name="card"> Пополняемый счет. </param>
        /// <param name="money"> Сумма пополнения. </param>
        public void Deposit(Card card, decimal money)
        {
            // Проверяем входные аргументы на корректность.
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            if (money <= 0)
            {
                throw new ArgumentException("Вносимая сумма должна быть больше нуля.", nameof(money));
            }

            // Вычисляем сумму сверхлимитной задолженности.
            var overdraft = card.CreditLimit - card.Credit;

            // Вычисляем насколько сумма пополнения перекрывает задолженность.
            var difference = money - overdraft;

            if(difference < 0)
            {
                // Если сумма пополнения не перекрывает задолженность,
                // то просто уменьшаем сумму задолженности.
                card.Credit += money;

                Console.WriteLine($"Ваш счет пополнен на сумму {money}. " +
                    $" Погасите задолженность в размере {difference} рублей. {card.ToString()}");
            }
            else
            {
                // Иначе закрываем задолженность, а оставшиеся средства переводим в собственные средства.
                card.Credit = card.CreditLimit;
                card.Debit = difference;

                // Переводим карту в состояние использования собственных средств.
                card.State = new UsingOwnFunds();

                Console.WriteLine($"Ваш счет пополнен на {money} рублей. " +
                    $"Кредитная задолженность погашена. {card.ToString()}");
            }
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

            if(price > card.Credit)
            {
                // Если сумма покупки больше, чем средства на счету,
                // от отказываем в операции.
                Console.WriteLine($"Операция не выполнена. Недостаточно средств на счете. {card.ToString()}");
                return false;
            }
            else
            {
                // Иначе расходуем кредитные средства.
                card.Credit -= price;

                // Вычисляем текущую задолженность.
                var overdraft = card.CreditLimit - card.Credit;

                Console.WriteLine($"Выполнена операция за счет кредитных средств на сумму {price}. " +
                    $"Погасите задолженность в размере {overdraft} рублей.  {card.ToString()}");

                // Вычисляем процент оставшейся суммы на счете.
                var limit = card.Credit / card.CreditLimit * 100;
                if(limit < 10)
                {
                    // Если оставшаяся сумма менее десяти процентов от кредитного лимита, то блокируем карту.
                    card.State = new Blocked();
                    Console.WriteLine($"Сумма на вашем счете составляет менее 10%. Ваш счет заблокирован. Пополните счет.  {card.ToString()}");
                }

                return true;
            }

        }
    }
}
