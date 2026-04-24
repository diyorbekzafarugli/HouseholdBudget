# 🏠 HouseholdBudget — Домашняя Бухгалтерия

REST API + Angular UI для управления личными финансами. Пользователи могут отслеживать свои доходы и расходы по категориям.

---

## 🛠 Технологии

### Backend

| Слой           | Технология                          |
| -------------- | ----------------------------------- |
| Backend        | ASP.NET Core 10, C#                 |
| Архитектура    | Clean Architecture + CQRS + MediatR |
| База данных    | PostgreSQL                          |
| ORM            | Entity Framework Core 10            |
| Аутентификация | JWT Bearer + Refresh Token          |
| Валидация      | FluentValidation                    |
| Логирование    | Serilog                             |
| Документация   | Swagger / OpenAPI                   |

### Frontend

| Слой       | Технология            |
| ---------- | --------------------- |
| Framework  | Angular 21            |
| UI Library | Angular Material      |
| Charts     | Chart.js + ng2-charts |
| HTTP       | Angular HttpClient    |
| State      | Angular Signals       |

---

## 📁 Структура Проекта

```
HouseholdBudget/
├── src/
│   ├── HouseholdBudget.Domain/          # Сущности, интерфейсы, исключения
│   ├── HouseholdBudget.Application/     # CQRS, валидаторы, DTO
│   ├── HouseholdBudget.Infrastructure/  # EF Core, репозитории, авторизация
│   └── HouseholdBudget.Api/             # Контроллеры, middleware, Program.cs
├── household-budget-ui/                 # Angular UI
│   └── src/app/
│       ├── core/                        # Services, Guards, Interceptors, Models
│       ├── features/
│       │   ├── auth/                    # Login, Register
│       │   ├── dashboard/               # Главная страница
│       │   ├── transactions/            # Транзакции
│       │   └── categories/              # Категории
│       └── shared/                      # Общие компоненты
├── dump.sql                             # Дамп базы данных
└── README.md
```

---

## ⚙️ Запуск Проекта

### Требования

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Node.js 20+](https://nodejs.org/)
- Angular CLI: `npm install -g @angular/cli`

---

### 0. Клонирование репозитория

```bash
git clone https://github.com/diyorbekzafarugli/HouseholdBudget.git
cd HouseholdBudget
```

### 🔧 Backend

#### 1. Настройка appsettings.json

Откройте файл `src/HouseholdBudget.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=household_budget;Username=postgres;Password=postgres"
  },
  "JwtSettings": {
    "Secret": "BORIBOYEV_DIYORBEK_ZAFAR_UGLI_1997_12_02_SECRET_KEY",
    "Issuer": "HouseholdBudget.API",
    "Audience": "HouseholdBudget.Client",
    "ExpiresInHours": 24
  }
}
```

#### 2. Запуск Backend

```bash
dotnet run --project src/HouseholdBudget.Api
```

База данных и миграции применяются **автоматически** при запуске.

#### 3. Swagger UI

```
https://localhost:7099/swagger
http://localhost:5168/swagger
```

---

### 🎨 Frontend (Angular UI)

#### 1. Установка зависимостей

```bash
cd household-budget-ui
npm install
```

#### 2. Запуск Frontend

```bash
ng serve
```

#### 3. Открыть в браузере

```
http://localhost:4200
```

> ⚠️ Backend должен быть запущен до открытия UI

---

## 🗄 База Данных

### Восстановление из SQL дампа

```bash
psql -U postgres -d household_budget < dump.sql
```

### Применение миграций вручную

```bash
dotnet ef database update \
  --project src/HouseholdBudget.Infrastructure \
  --startup-project src/HouseholdBudget.Api
```

---

## 📡 API Эндпоинты

### 🔐 Аутентификация

| Метод | Эндпоинт                  | Описание          |
| ----- | ------------------------- | ----------------- |
| POST  | `/api/auth/register`      | Регистрация       |
| POST  | `/api/auth/login`         | Вход в систему    |
| POST  | `/api/auth/refresh-token` | Обновление токена |

### 👤 Пользователи

| Метод  | Эндпоинт                        | Описание             |
| ------ | ------------------------------- | -------------------- |
| GET    | `/api/users/me`                 | Текущий пользователь |
| PUT    | `/api/users/me`                 | Обновление данных    |
| PUT    | `/api/users/me/change-password` | Смена пароля         |
| DELETE | `/api/users/me`                 | Удаление аккаунта    |

### 📂 Категории

| Метод  | Эндпоинт               | Описание           |
| ------ | ---------------------- | ------------------ |
| GET    | `/api/categories`      | Все категории      |
| POST   | `/api/categories`      | Создать категорию  |
| PUT    | `/api/categories/{id}` | Обновить категорию |
| DELETE | `/api/categories/{id}` | Удалить категорию  |

### 💰 Транзакции

| Метод  | Эндпоинт                 | Описание                               |
| ------ | ------------------------ | -------------------------------------- |
| GET    | `/api/transactions`      | Список транзакций (фильтр + пагинация) |
| GET    | `/api/transactions/{id}` | Одна транзакция                        |
| POST   | `/api/transactions`      | Создать транзакцию                     |
| PUT    | `/api/transactions/{id}` | Обновить транзакцию                    |
| DELETE | `/api/transactions/{id}` | Удалить транзакцию                     |

### Параметры Фильтрации

```
GET /api/transactions?type=1&categoryIds=guid1&dateFrom=2024-01-01&dateTo=2024-12-31&pageNumber=1&pageSize=20
```

| Параметр      | Тип      | Описание                               |
| ------------- | -------- | -------------------------------------- |
| `type`        | int      | 1=Доход, 2=Расход                      |
| `categoryIds` | guid[]   | Категории (множественный выбор)        |
| `dateFrom`    | datetime | Дата начала периода                    |
| `dateTo`      | datetime | Дата конца периода                     |
| `pageNumber`  | int      | Номер страницы (по умолчанию: 1)       |
| `pageSize`    | int      | Записей на странице (по умолчанию: 20) |

---

## 🎨 UI Страницы

### 🔐 Авторизация

- Регистрация нового пользователя
- Вход в систему
- Автоматическое обновление JWT токена

### 📊 Главная (Dashboard)

- Общая сумма доходов
- Общая сумма расходов
- Текущий баланс
- Количество транзакций
- Круговая диаграмма: доля расходов от доходов
- Последние 5 транзакций

### 💰 Транзакции

- Список всех транзакций в таблице
- Фильтрация по типу, категориям и периоду
- Пагинация (10/20/50 записей на странице)
- Добавление новой транзакции (диалог)
- Редактирование транзакции
- Удаление транзакции
- Итоги: доходы, расходы, баланс

### 📂 Категории

- Список категорий (доходы и расходы отдельно)
- Добавление новой категории
- Редактирование категории
- Удаление категории
- Защита дефолтных категорий от изменения

---

## 🌍 Многоязычность

API возвращает сообщения об ошибках валидации на 3 языках:

```
Accept-Language: ru    # Русский
Accept-Language: uz    # Узбекский
Accept-Language: en    # Английский (по умолчанию)
```

---

## 🔒 Аутентификация

Все эндпоинты (кроме auth) требуют JWT токен:

```
Authorization: Bearer ВАШ_JWT_ТОКЕН
```

**Порядок работы:**

1. Зарегистрируйтесь через `/api/auth/register`
2. Войдите через `/api/auth/login` — получите `accessToken` и `refreshToken`
3. Используйте `accessToken` в заголовке `Authorization`
4. При истечении токена обновите через `/api/auth/refresh-token`

---

## 🏗 Архитектура

```
Request → Controller → MediatR → ValidationBehavior → UserExistsBehavior → Handler → Repository → DB
```

### Слои

- **Domain** — бизнес-правила, сущности, интерфейсы
- **Application** — CQRS команды/запросы, валидаторы, DTO
- **Infrastructure** — EF Core, реализации репозиториев, JWT, BCrypt
- **API** — контроллеры, middleware, расширения
- **UI** — Angular 21 + Angular Material

---

## ✅ Реализованный Функционал

### Backend

- ✅ Аутентификация через JWT + Refresh Token
- ✅ Clean Architecture + CQRS + MediatR
- ✅ Мягкое удаление (Soft Delete)
- ✅ Пагинация результатов
- ✅ Многоязычная валидация (ru, uz, en)
- ✅ Глобальная обработка исключений
- ✅ Rate Limiting (защита от спама)
- ✅ Health Checks (`/health`)
- ✅ Структурированное логирование (Serilog)
- ✅ Форматирование чисел (`1 500 000.00`)
- ✅ Фильтрация по типу, категориям и периоду
- ✅ Начальные категории доходов и расходов

### Frontend

- ✅ Angular Material UI
- ✅ JWT Interceptor + автообновление токена
- ✅ Глобальная обработка ошибок (SnackBar)
- ✅ Круговая диаграмма расходов от доходов
- ✅ Фильтрация и пагинация транзакций
- ✅ Диалоговые окна для форм
- ✅ CORS настройка

---

## 📋 Начальные Категории

### Доходы

- Заработная плата
- Иные доходы

### Расходы

- Продукты питания
- Транспорт
- Мобильная связь
- Интернет
- Развлечения

---

## 👨‍💻 Автор

**Diyorbek**  
.NET Developer
