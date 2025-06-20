Пример процесса принятия заявки от пользователя

📄 Пример заявки
Заявка № 2025-005
Дата получения: 2025-04-10 14:23
От: ivan.petrov@example.com
Тема: Не удаётся отметить явку участника
Описание:

При попытке отметить явку демонстратору ошибка: “Запись уже существует”, хотя у участника ещё нет записи в таблице. Прошу проверить логику сохранения.

Этап 1. Принятие и регистрация заявки

| Поле      | Значение                                                          |
| --------- | ----------------------------------------------------------------- |
| Приемщик  | [support@eventregistry.local](mailto:support@eventregistry.local) |
| Время     | 2025-06-10 14:25                                                  |
| Приоритет | Средний (Application issue, non-urgent)                           |
| Категория | Функциональный (регистратор, UI/DAL)                              |


Номер заявки присвоен автоматически и соответствует требованиям ГОСТ по регистрации обращений

Этап 2. Анализ и диагностика
· Изучение логов приложения (DAL): факт вызова ExistsAsync — возвращает true, хотя записи нет.

· Проверено: параметр ParticipantId + EventId не найден; логика DAL при ошибке вставки срабатывает некорректно.

Причина выявлена: SQL-запрос на вставку содержит IF EXISTS(...) RETURN; без возврата к вызывающему коду.

Этап 3. Реализация решения
· Исправлен метод в AttendanceRepository:

csharp

if (await ExistsAsync(...))
  throw new InvalidOperationException("Явка уже отмечена");

· Добавлено локальное отображение MessageBox с текстом, эквивалентным исключению.

· Добавлены unit-тесты для ExistsAsync и MarkAttendanceAsync, покрывающие сценарий повторной вставки.

Этап 4. Проверка и подтверждение
· Закрытая сборка версии 1.2.3-fix передана тестировщику.

· Прогон тест-кейса:
1.Создать участника и мероприятие.
2.Отметить явку — успешно.
3.Повторно — появляется MessageBox «Явка уже отмечена».

· Результат: заявка исправлена — отмечено в баг-трекере как «Closed, verified» (2025-06-11 09:15).

Этап 5. Ответ пользователю

Ответ:

Здравствуйте, Иван!
Спасибо за обратную связь по заявке № 2025-005.
Ошибка исправлена: теперь сообщение корректно обрабатывается, и при повторной попытке показать окно:
«Явка уже отмечена».
Новая версия опубликована под номером 1.2.3‑fix, пожалуйста, обновите приложение.
Благодарим за помощь в улучшении Event Registry!

Ответ отправлен: 2025-02-11 09:17.

Этап 6. Завершение и анализ
· Заявка закрыта — выполнено решение и проведено уведомление пользователя.

· Информация внесена в регистр заявок

· Проведен анализ причины — вынесен вывод о необходимости покрытия тестами всех DAL-методов

· Обновлены тестовые сценарии и добавлено предупреждение в технической документации.