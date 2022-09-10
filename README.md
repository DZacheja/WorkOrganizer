# WorkOrganizer
Program do zarządzania realizacją projektów.

## Obecna funkcjonalność programu:
- Logowanie i tworzenie nowych użytkowników
- Zapisywanie danych do lokalnej bazy danych PostgreSQL
- Podział na zleceniodawców / Zlecenie / Podzadania
- Dodawanie wiadomości do danego podzadania
- Możliwość filtrowania po wiadomościach
- Możliwość tworzenia wieloetapowych podzadań
- Zapisywanie ulubionych filtrów

## Cele do zrealizowania
- Refaktoryzacja kodu
- Przeniesienie bazy danych na Serwer VPS
- Stworzenie osobnego WebAPI do komunikacji
- Poprawa wizualna
- Dodanie czatu do zadań
- Dodanie możliwości przechowywania zdjęć i plików w bazie danych
- Wysyłanie maili przypominających o kończącym się terminie do wykonania zadania

## Prezentacja wideo:


https://user-images.githubusercontent.com/72301310/189498098-02bf8516-406b-4c77-86db-91856bec609d.mp4


## Prezentacja szczegółowa aplikacji:

Poniżej znajdują się zrzuty ekranu z obecnego stanu aplikacji:

### Okno główne:

Tak wygląda ekran po włączeniu: 

<p align=left>
<img src ="https://user-images.githubusercontent.com/72301310/189498260-20b8161e-703a-47a7-befc-e7122e1ede9e.png" Width=600>
</p>

### Okno logowania
Istnieje tu możliwość zalogowania do aplikacji lub utworzenia nowego użytkownika w bazie danych.<br>
Po zalogowaniu, dane do logowania są zapisywanie w folderze programu w pliku txt zaszyfrowanym kluczem symetrycznym
<p align=left>
<img src ="https://user-images.githubusercontent.com/72301310/189498395-1c3cc051-9df1-43c9-a33b-c6bf15ce4e98.png" Width=600>
<img src ="https://user-images.githubusercontent.com/72301310/189498411-1072159c-927b-4dd7-a75a-8d6b27728d9d.png" Width=600>
</p>

### Przeglądanie zadań
Po wybraniu zakładki zadania wyświetlają się wszystkie aktywne zadania ze wszystkich robót, posortowane terminem realizacji.<br>
Po prawej stronie ekranu mamy możliwość filtracji wyświetlanych zadań.<br>
Zadania nieaktywne są wygaszone względem aktywnych, po najechaniu na zadanie mamy informacje, który użytkownik odznaczył dany element.<br>
Dodawanie nowego zadania odbywa się po kliknięciu na kręcący się kalendarz. Aby poprawnie dodać nowy element należy wcześniej w miejscu filtrowania wybrać odpowiedniego zleceniodawcę, robotę i rodzaj.<br>
Istnieje możliwość zapisania wybranego filtru do listy ulubionych filtrów. Można filtrować po wszystkich elementach oraz treści a schemat listy jest zapisany w pliku txt również zaszyfrowanym kluczem symetrycznym

<p align=left>
<img src ="https://user-images.githubusercontent.com/72301310/189498587-1af862cf-dc39-40f1-94dc-5b1f5c25010f.png" Width=600>
<img src ="https://user-images.githubusercontent.com/72301310/189498598-ace8e1e5-f967-46ca-acf8-b614777c4659.png" Width=600>
</p>

### Dodawanie nowego zadania
W oknie dodawania nowego zadania mamy możliwość definicji zarówno nowego zleceniodawcy, roboty jak i wszystkich jej elementów. <br>
Ze względu na wykorzytywanie tego programu w firmie, której każde zlecenia składa się z tych samych elementów ich wybór następuje w liście gotowych elementów. <br>
Każda robota ma swój indywidualny kolor co ułatwia przeglądanie zadań.
<p align=left>
<img src ="https://user-images.githubusercontent.com/72301310/189498770-3d4d4f63-0324-4689-afc2-dc2e48376386.png" Width=600>
</p>

## Autor
<b>Damian Zacheja</b>

## Licencja
![Licencja]()


