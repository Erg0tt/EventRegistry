Руководство разработчика

1. Стандарты оформления кода и документации
1.1 Кодовые соглашения (ГОСТ/ISO + Microsoft)
· Соблюдаем Microsoft C# Naming Conventions:

· PascalCase для классов, методов, свойств.

· camelCase для локальных переменных и параметров.

· Используем var только при явном типе справа (var userList = new List<User>();).

· Отступ — 4 пробела, не табуляция.

· Максимальная длина строки — 120 символов.

· Объясняющие комментарии и XML-документация (/// <summary>...).

· Использование DOTNET_FORMAT для автоформатирования (dotnet format --fix).

1.2 Документация (по ГОСТ 19.*)

· Каждый модуль снабжён заголовком:

//---------------------------------------------------------------------
// Module: EventRepository.cs
// Purpose: CRUD-операции для Events
// Author:  Иванов И.И.
//---------------------------------------------------------------------
· Основные разделы:

1.Назначение и интеграция модуля.
2.Интерфейсы (методы, параметры).
3.Алгоритм работы.
4.Исключения и ошибки.
5.Примеры использования.
6.Зависимости.

2. Сборка и работа с репозиторием
2.1 Репозиторий и ветвления
· Gitflow‑подобная схема: main — продакшен, develop — интеграция, feature/* — новые задачи.

· Каждый feature/* перед слиянием — PR с описанием, релевантным стандартам.

2.2 Сборка и CI
· Решение — .sln:

· EventRegistry.Core

· EventRegistry.DAL

· EventRegistry.UI

· EventRegistry.Tests

· Команда сборки:

bash 
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
dotnet tool run dotnet-format --check

· CI: GitHub Actions / Azure Pipelines — сборка + тесты + dotnet-format.

3. Документация API и модулей
· Используем XML-комментарии <summary>, <param>, <returns>.

· Генерация: Swagger для REST‑API (если будет WebAPI).

· Общий документ – DesignOverview.md:

· Архитектура MVVM.

· Связь слоёв (DAL, ViewModel, UI).

· Содержание директорий, шаблоны имен.

4. Конфигурация и миграции БД
· Строка подключения лежит в App.config:

xml

<connectionStrings>
  <add name="DefaultConnection" connectionString="Data Source=.;Initial Catalog=EventRegistry;Integrated Security=True"/>
</connectionStrings>

· Миграции – через скрипты Migrations/XXX_CreateSchema.sql либо EF Core Migrations.

· Сценарий развертывания БД: SetupDb.bat.

5. Тестирование
5.1 Unit‑тесты
·Используем xUnit.

· Целевая ветвь покрытия: > 60% критичных модулей (DAL, ViewModel).

· Структура: Arrange / Act / Assert, mocking через Moq.

· Тестовые данные — через SQLite или InMemory-провайдер.

5.2 Интеграционные и UI‑тесты
· Интеграционные: запускаются против SQL Express LocalDB, тестируют сквозные операции.

· UI‑тесты: можно при необходимости использовать UI Automation или Appium.

· Тестовые отчёты + видео (где применимо).

6. Менеджмент конфигурации
· Каждое изменение — коммит с описанием feat:, fix:, refactor:.

· Версия проекта определяется по SemVer (Major.Minor.Patch).

· Теги при релизе: v1.0.0.

7. Процедура выпуска релиза

1.Собрать Release.
2.Убедиться, что CI успешно прошёл.
3.Обновить версию, записать Release Notes в CHANGELOG.md.
4.Создать tag и GitHub Release.
5.Сгенерировать сборочный артефакт (ZIP / msi).
6.Обновить документацию (ключевые изменения, установщик, настройки).

8. Поддержка и сопровождение
· Выделена ветка hotfix/* для быстрого исправления багов в продакшене.

· Для ошибокSEV1 — реакция до 1 рабочего дня, новый релиз не позднее дня.

· Обратная связь — через Issues.

