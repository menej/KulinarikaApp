# KulinarikaApp
Project made by: Enej Mastnak 

Aktivna rešitev spletne aplikacije: https://kulinarika.azurewebsites.net/ 

Aktivna rešitev spletne storitve REST: https://kulinarika.azurewebsites.net/api/v1/recipe 

Dokumentacija spletne storitve REST: https://kulinarika.azurewebsites.net/swagger/index.html 
<hr>

<p>
Informacijskim sistem <em>Kulinarika</em> omogoči domačim kuharjem lažji dostop do domačih kuharskih receptov.
</p>

![slika](https://user-images.githubusercontent.com/68961385/210558087-16b40298-c5ef-4ecf-9e38-a2fd5c32484f.png)

<p>
Aplikacija omogoča uporabnikom prijavo in registracijo oziroma možnost ustvarjanja lastnega računa in objavo lastnega recepta. Poleg tega imajo tako kot prijavljeni uporabniki tudi neprijavljeni uporabniki možnost na vpogled in iskanje poljubnih kuharskih receptov. Za lažje iskanje po različnih obstoječih je omogočeno tudi iskalno okence, kjer uporabnik lahko vpiše neko ključno besedo ali beseda, za iskanje recepta. 
</p>

![slika](https://user-images.githubusercontent.com/68961385/210558153-3c3803c3-c55e-40a0-88a5-5260c3b41900.png)

<p>
Ob pritisku na naslov recepta, se odpre okence, ki prikaže naslov, avtorja in besedilo recepta. Prijavljeni uporabnik ima v tem oknu tudi možnost spreminjanja in brisanja svojega recepta. Pri tem pa ko si bodo lahko preostali (prijavljeni in neprijavljeni) uporabniki recept le ogledovali. Prijavljeni uporabniki bodo pri tem imeli tudi možnost, da ob receptu pustijo svoj komentar. V sistemu je prisotna tudi vloga moderatorja, ki ima možnost brisanja nezaželenih receptov oziroma komentarjev.
</p>

![slika](https://user-images.githubusercontent.com/68961385/210558229-0258b1af-22ff-49a8-8fc0-5cf37ca8a770.png)

<p>
Prijavljeni uporabniki imajo možnost hranjenja določenega recepta v zaznamke, kjer se nato povezava do recepta shrani v zavihek »Zaznamki« posameznega uporabnika.
</p>

![slika](https://user-images.githubusercontent.com/68961385/210558319-0c53ef98-7989-4781-b512-e7f1f7a19a16.png)

<p>
Aktivna verzija aplikacije trenutno podatke shranjuje v podatkovni bazi oziroma na SQL strežniku, ki je na voljo v Microsoft Azure oblaku. Podatkovna baza v celoti sestoji iz 10 tabel. Spodaj je prikazan diagram podatkovnega modela podatkovne baze, ki je bil zgrajen z orodjem SQL Server Management Studio.
</p>

![slika](https://user-images.githubusercontent.com/68961385/210558537-6b222b67-6e76-4051-8366-0dae6e5a4c36.png)

<p>
Poleg .NET aplikacije je implementiran tudi .NET REST spletna storitev oziroma API vmesnik, sprejema in pošilja podatke v JSON formatu. Spletna storitev podpira vse CRUD operacije, kar se tičejo entitete Recipe, toda je za dostop do nje potreben poseben ključ, saj je implementirana avtentikacija oziroma avtorizacija zahtevkov. Objavljena je tudi dokumentacija storitve z uporabo Swagger UI. 
<br>
Omenjeno REST spletno storitev uporablja tudi Android odjemalec, ki lahko komunicira oziroma pridobi podatke »Read« o receptih iz API vmesnika. To komunikacijo z odjemalcem in z storitvijo REST omogoča knjižnica Volley. V aplikaciji bo lahko v prihodnosti uporabnik Android aplikacije objavil »Create« tudi svoj recept z uporabo te storitve, toda je trenutno ta del nedelujoč.  
</p>

![slika](https://user-images.githubusercontent.com/68961385/210558614-09912146-5636-45e6-be46-66ab08a5b6e0.png)
