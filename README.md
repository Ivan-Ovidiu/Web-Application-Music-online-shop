# Music-online-shop Web application
Dezvoltata in C# .NET Core si stilizata cu Bootstrap, 
aceasta aplicatie web a fost realizata cu scopul de a vinde produse muzicale(CD,vinil,casete, etc.) si
de a creea o interfata user-friendly prin intermediul careia clientii pot achizitiona cu usurinta ceea ce isi doresc.

![image](https://github.com/user-attachments/assets/403344d6-48d3-4f11-a455-cff606e71e6b)

# Roluri utilizatori

1.Administrator:
* Controleaza toate produsele si categoriile
* Aproba sau respinge produse noi
* Administreaza categoriile si poate crea unele noi
* Poate sterge sau modifica produse si comentarii
* Gestioneaza rolurile utilizatorilor

2.Colaborator:
* Adauga produse noi (necesita aprobare)
* Poate modifica produsele proprii
* Dupa modificare, produsul trebuie aprobat din nou

3.Utilizator inregistrat:
* Adauga produse in cos
* Poate lasa review-uri si rating-uri
* Modifica propriile review-uri
* Vede toate produsele si comentariile

4.Utilizator neinregistrat:
* Doar vizualizeaza produse si comentarii
* Este directionat spre inregistrare cand vrea sa cumpere

# Functionalitati Principale:

* Adaugarea Produselor:<br />
Utilizatorii colaborator si admin pot adauga produse in magazin. Aceste produse trebuie aprobate de administrator inainte de a fi vizibile. Administratorul are optiunea de a respinge sau aproba aceste cereri.
* Managementul Categoriilor:<br />
Produsele sunt organizate in categorii create dinamic de catre administrator. Acesta poate adauga noi categorii direct din interfata aplicatiei, precum si sa vizualizeze, editeze sau stearga categoriile existente.
* Detalii Produse:<br />
Fiecare produs contine urmatoarele informatii obligatorii: titlu, descriere, poza, pret, stoc si rating. Sistemul include un mecanism de rating de la 1 la 5 stele. Fiecare utilizator poate acorda un singur rating per produs, iar scorul final este calculat pe baza tuturor rating-urilor existente.
* Review-uri:<br />
Produsele pot primi review-uri de tip text de la utilizatori. Review-ul include un comentariu text last de utilizator. Acest camp este optional, dar restul campurilor sunt obligatorii. Stocul reprezinta numarul de produse disponibile.
* Editare Produse:<br />
Utilizatorul colaborator poate sa editeze si sa stearga produsele adaugate de el. Dupa editare, produsul necesita din nou aprobare din partea administratorului.
* Cautare Produse:<br />
Produsele pot fi cautate dupa denumire prin intermediul unui motor de cautare. De asemenea, produsele pot fi cautate dupa nume si trebuie sa fie gasite si in cazul in care utilizatorul cauta doar anumite parti care compun denumirea.
* Sortare si Filtrare:<br />
Rezultatele motorului de cautare pot fi sortate crescator sau descrescator in functie de pret si numarul de stele. Sistemul implementeaza filtre din care utilizatorul poate alege optiunile dorite.
* Administrare:<br />
Administratorul poate sterge si edita atat produse cat si comentarii. Acesta poate, de asemenea, sa activeze sau sa revoce drepturile utilizatorilor.

# Instructiuni de setup:
## Preconditii:
* Visual Studio 2022 or later<br>
* .NET 6.0 SDK or later<br>
* SQL Server (Local or Express)<br>
## Database Setup:
> [!NOTE]
> Foloseste aceste comenzi in terminal<br>
> Add-migration [nume_migratie]<br>
> Update-Database<br>
## Configuratia rolurilor:
### Aplicatia vine cu urmatoarele roluri predefinite:<br>
* Administrator<br>
* Collaborator<br>
* UserN (Normal User)<br>
* UserI (Identified User)<br>
### Default admin credentials:<br>
* Email: admin@test.com<br>
* Parola: Admin1!<br>
## Rulare aplicatie:
* Open the solution in Visual Studio<br>
* Restore NuGet packages<br>
* Build the solution<br>
* Run the application<br>

# Exemple (screenshots)
Adminul poate modifica rolurile utilizatorilor:
![image](https://github.com/user-attachments/assets/de318914-90b0-46f9-a4ee-5fb123ce9f25)

Adminul sau colaboratorii pot adauga produse noi in baza de date a aplicatiei web:
![image](https://github.com/user-attachments/assets/8647feaa-7571-4575-b30a-7b5b9744ab27)
![image](https://github.com/user-attachments/assets/93b8bdf5-1222-4e6f-b614-0ba8fc24119e)

Doar adminul le poate aproba/respinge:
![image](https://github.com/user-attachments/assets/69fd5a71-7e8c-44ad-8793-e4db208cf0ae)

Dupa aprobare articolul va aparea pe pagina principala vizibil pentru toti utilizatorii:
![image](https://github.com/user-attachments/assets/82d855d3-0a22-41fd-88de-0a814766bb3e)





 
