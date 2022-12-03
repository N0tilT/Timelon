<p align="center">
      <img src="https://i.ibb.co/rGjp0C5/2022-12-03-140149554.png" alt="Project Logo" width="300">
</p>

<p align="center">
   <img src="https://img.shields.io/badge/.NET%20ver-4.7.2-informational" alt=".NET Version">
   <img src="https://img.shields.io/badge/App%20ver-1.0-blueviolet" alt="App Version">
   <img src="https://img.shields.io/badge/license-Apache%202.0-green" alt="License">
</p>

## About
Список задач — программа для управления проектами. Её функционал позволяет пользователю следить за рабочими процессами и контролировать выполнение поставленных задач. 

Цель проекта: Создать удобный список задач, в котором можно создавать уникальные задачи и группировать их по  спискам с возможностью отслеживать статус их выполнения. 

## Возможности

- **Карточка дела** (карточка, карта) - набор характеристик карточки, который включает в себя краткую постановку задачи (название), описание задачи, дату последнего обновления, отметку о выполнении.
- - Отметка о важности. Выделяет задачу среди всех других. Позволяет пользователю определить приоритетные для себя дела.
- - Отметка о выполнении. Определяет статус задачи: В работе или выполнена

- **Список карт** (список, список задач) - карты, сгруппированные пользователем в отдельный список. Позволяет хранить несколько карт, сортировать их по различным параметрам, осуществлять поиск карт по части названия или описания.

- **Список списков карт** (менеджер списков) - список, содержащий все списки карт. Позволяет хранить списки карт, осуществлять поиск по части названия или описания по всем содержащимся в нём спискам.

- **Добавление/Удаление карт,списков**

- **Выполнение задачи(карты)** Отметить задачу как выполненную. Есть возможность просмотреть все выполненные задачи в выбранном списке

- **Сортировка карт** Есть возможность сортировать карты в разном порядке и по разным параметрам: по дате, по выполненности и по важности

### Примеры задач:

1. Отслеживание дел по дому (уборка, покупки, подарки близким и т.п.)
2. Отслеживание рабочих проектов (Формирование списка чётких задач, алгоритма, которому можно следовать и поэтапно выполнять поставленную задачу)
3. Постановка глобальных целей (научиться программировать, закончить университет, изучить французский язык ) и отслеживание пути к их выполнению (добавление тематических задач: записаться на языковые курсы и т.п.)


## Documentation
### Manager
Основной класс. Отвечает за сохранение и загрузку данных, а также хранение объектов, полученных из этих данных (Следует иметь ввиду, что при использовании Менеджера в своих решениях, желательно инициализировать экземпляр класса как можно раньше, если он не был инициализирован до этого.)

- **-** **`GetList`** (Получение списка)
- **-** **`SetList`** (Вставка Списка)
- **-** **`RemoveList`** (Удаление Списка)
- **-** **`ContainsList`** (Проверка на существование Списка)
- **-** **`SearchByContent`** (Поиск по контенту во всех Списках)
- **-** **`InjectEssentials`** (Внедрить списки по умолчанию)

Работа с данными (XML):
- **-** **`Sync`** (Сохранить данные в файл)
- **-** **`Load`** (Загрузить данные из файла)

### Randomizer
Работа с генератором псевдо-случайных чисел. 
- **-** Статический доступ к глобальному генератору псевдо-случайных чисел (Random)
- **-** **`NextBool`**  (Получение следующего случайного булевого значения)
- **-** **`NextString`**  (Получение следующей случайной строки заданной длины)
- **-** **`NextCollectionIndex`**  (Получение следующего случайного индекса заданной коллекции)
- **-** **`NextDateTime`**  (Получение следующих даты и времени (DateTime))

### TimelonCl.Data
Пространство имён. Хранит в себе основные классы, экземпляры которых участвующит в обороте данных между постоянной и оперативной памятью.

Абстрактные классы:
- **-** **`Unique`** (Уникальный Класс). Генерация уникальных идентификаторов в текущей сессии
- **-** **`DataContainer`** (Контейнер Данных). Сериализация в XML

Инкапсулирующие классы:
- **-** **`DateTimeContainerData`** (Данные Контейнера Дат)
- **-** **`CardData`** (Данные Карты)
- **-** **`CardListData`** (Данные Списка Карт)

- **-** **`DateTimeContainer`** (Контейнер Дат)
- **-** **`Card`** (Карта)
- **-** **`CardList`** (Список Карт)

### DateTimeContainer
Контейнер Дат служит для хранения в себе нескольких экземпляров класса DateTime:

- **-** **`Created`** (дата создания)
- **-** **`Updated`** (дата обновления или null)
- **-** **`Planned`** (запланированная дата или null)
Стоит обратить внимание на то, что дата обновления и запланированная дата 
напрямую зависят от даты создания - они не могут превышать ее. 
Соответственно, при изменении даты создания зависимые от нее даты будут перепроверены автоматически.

### Card
Карта наследует Уникальный Класс и представляет собой карточку дела. По большей части этот класс
содержит в себе основные поля, характерные для карточки.

### CardList

Список Карт, также как и Карта, наследует Уникальный Класс и представляет собой список карточек
дел. В качестве способа хранения экземпляров Карт в классе используется Dictionary (Библиотека).

- **-** **`Get`** (Получить карту)
- **-** **`Remove`** (Удалить Карту)
- **-** **`Set`** (Сохранить Карту)
- **-** **`Contains`** (Проверить наличие Карты)

Библиотека позволяет мгновенно получить Карту через ее идентификатор вне
зависимости от количества хранимых Карт, достаточно произвести сортировку один раз и занести
идентификаторы в отдельный список для быстрого доступа.
- **-** idListDefault (идентификаторы по дате обновления)
- **-** idListImportant (идентификаторы по статусу важности)
- **-** idListCompleted (идентификаторы по статусу выполнения)

Этим спискам соответствуют методы:
- **-** **`GetListDefault`** (Получение Карт, отсортированных по дате обновления)
- **-** **`GetListImportant`** (Получение Карт, отсортированных по статусу важности)
- **-** **`GetListCompleted`** (Получение Карт, отсортированных по статусу выполнения)

Управление статусом сортировки. Перечисление SortOrder
(Направление Сортировки):

- **-** Initial (Карты необходимо отсортировать (используется только внутри класса))
- **-** Unsorted (В произвольном порядке)
- **-** Ascending (По возрастанию)
- **-** Descending (По убыванию)

Соответственно, с помощью этого перечисления при использовании методов получения отсортированных
Карт существует возможность указать необходимый порядок.

## Developers

- [T.Latypov](https://github.com/N0tilT)
- [A.Simonov](https://github.com/dubstepTractor)
- [M.Cherepan](https://github.com/PolShestogo)
- [V.Ponomarev](https://github.com/vadimyt)
- [P.Gromova](https://github.com/jowlly)

## License
Проект Timelon распространяется по лицезии Apache 2.0
