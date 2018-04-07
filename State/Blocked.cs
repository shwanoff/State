using System;

namespace State
{
    /// <summary>
    /// Заблокированное состояние счета.
    /// </summary>
    public class Blocked : IState
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
            var difference =  money - overdraft;

            if (difference < 0)
            {
                // Если сумма пополнения не перекрывает задолженность,
                // то просто уменьшаем сумму задолженности.
                card.Credit += money;

                // Вычисляем процент оставшейся суммы на счете.
                var limit = card.Credit / card.CreditLimit * 100;
                if (limit < 10)
                {
                    // Если после пополнения на счете все еще меньше десяти процентов от лимита,
                    // то просто сообщаем об этом пользователю.
                    Console.WriteLine($"Ваш счет пополнен на сумму {money}. " +
                        $"Сумма на вашем счете все еще составляет менее 10%. Ваш счет остался заблокирован. Пополните счет на большую сумму.  {card.ToString()}");
                }
                else if (limit >= 10 && limit < 100)
                {
                    // Если задолженность перекрыта не полностью, то переводим в состояние расходования кредитных средств.
                    card.State = new UsingCreditFunds();

                    Console.WriteLine($"Ваш счет пополнен на сумму {money}. Задолженность частично погашена. " +
                        $"Погасите задолженность в размере {Math.Abs(difference)} рублей. {card.ToString()}");
                }
                else
                {
                    // Иначе задолженность полностью погашена, переводим в состояние расходования собственных средств.
                    card.State = new UsingOwnFunds();

                    Console.WriteLine($"Ваш счет пополнен на {money} рублей. Задолженность полностью погашена. {card.ToString()}");
                }
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
        /// <returns> Успешность выполнения операции.</returns>
        public bool Spend(Card card, decimal price)
        {
            // Отказываем в операции.
            Console.WriteLine($"Ваш счет заблокирован. Пополните счет.  {card.ToString()}");
            return false;
        }
    }
}
