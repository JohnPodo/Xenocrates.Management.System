Summary Of Xenocrates
Xenocrates is an application designed for providing companies with an employee management system to ensure better organization. Xenocrates is built in Asp.Net MVC with SQL server as database and JQuery for async Data Requests.
Xenocrates’ services are divided in three main categories

Admin Services:
•	Statistics for the whole company that keeps him up to date for his company.
•	View the count of the employees in each role.
•	CRUD Operations on all workers in his company, navigate to find every detail he wants about his employees
•	CRUD Operations on all of his departments and see specific details of each department
•	View all active but also finished projects in all of his departments with details.
•	Integrated Payment system with Paypal that allows him to pay fast with a click of a button.
•	Analytic payment history for each employee.
Supervisor Services:
•	Statistics of his department.
•	View Details of each worker in his department
•	CRUD Operations on individual calendars for his employees
•	CRUD Operations on projects
•	Ability to organize and communicate with Live Chat

Employee Services:
•	Communication with everyone on his Department with Live Chat
•	View his calendar to see his schedule for the month
•	View and finalize the projects assigned to him

Services Details: 
•	Paypal right now is integrated in sandbox mode but has all the potentials to go live. For the moment Xenocrates cooperates with the payouts service of Paypal API. Payment system for Xenocrates is built so it can protect Admin from making a mistake in payments. After an employee is paid, a message appears that reminds him that Admin has already paid the specific employee and does not allow him to pay him again for the duration of the month. After every attempt for payment, the system detects the status of the request and if it’s positive, it saves the successful payment to the system and stores it in employee’s personal data so that Admin can have a full track of his payments, otherwise system announces that was a problem with the transaction.
•	Email and Phone inserted for each employee is searched in global scale to check if they are valid so no fake accounts are registered. API’s for that include NeutronApiPhone and ZeroBounce1.
•	Security method that runs frequently to make sure that no inappropriate entries are saved inside Database
•	Live chat with implementation on SignalR technology that allows every employee of a Department to chat with each other in real time. Every Message is saved on database so it can be added to message history.
•	Calendar implementation with Full Calendar as base but fully personalized for the needs of Supervisor. Supervisor inspects the projects he assigned to each employee and depending on the work load adjusts the monthly schedule for each one of the employees. On the other hand, employee can only see his calendar without being able to perform actions to it.
•	Charts and Graphics with personalized vanilla Javacript and Ajax calls with JQuery to fit the requirements.
•	Mobile Phone is confirmed using Twillio with a Free Trial account. Mobile is inserted and confirmed at the first login of every new user. First an API validates that phone number exists and then a message is sent from Twillio with a security code to proceed with safety. To ensure that Twillio confirmation works fill free to contact Team Pyravlos so that we can include your number in our account cause Free-Trial accounts only send texts to verified numbers from the account.
•	Email confirmation is implemented with the help of built in methods that are provided from Asp.Net that allows us to send a new confirmation email to every new employee to be sure that their email is confirmed.

Back-End Analysis: 
Xenocrates is built with the idea of a main component that is provided with data and services from independent providers. Controllers do not have access to database or pull the weight of difficult tasks. So every controller has a data repository of its own and also access to a service that does all the tasks. Therefore the data repository for each controller is also divided into different groups depending on the type of data.
 

Separation of concerns was our main focus while building Xenocrates so we could have a lot of independent methods that construct a quick, maintainable and integrated system.
Methods that have single responsibilities so source code is easier to understand and leave a window for XUnit testing in a future patch so everything will be more controlled.

SortingAndFiltering repository and also a repository for the Viewbags were implemented following the data repository pattern. Everything that needs to be done so that the rule of “Don’t repeat yourself” and the separation of concerns is achieved.






Database Schema: 
 
Database was designed and created with a main focus, interact with Entity Framework Entities as little as possible because the complexity of entity with the keys that provides and binds its entities could be very dangerous for the stability of our database. We tried to normalize our database as much as possible so that everything could be manipulated without causing a tsunami of reactions. In most cases we succeeded that in dividing our entities to two main entities. The first one was the Application User made and protected from the Entity-Framework that represents the online profile of an employee and the personal details of an employee that in our database is recognized in the table of Worker. Everything except these two entities can be deleted or edited any time without any cost.


Problems we faced:
 We ran into different kinds of small problems:
•	Datetime manipulation and display
•	Convert C# Datetimes to Javascript Date
•	Manipulation of template we used for front end
But the problems that troubled us more:
•	Alpha version of Xenocrates was built around table of Asp.NetUsers and after adding foreign keys and relationships between the Frameworks entities and custom entities made from us the system started to complain because the keys assigned to Asp.NetUsers were exposed. So we had to start again and find the way to the final design of the database. That is why the folder is named ManagementSystemVersionTwo.
•	Full Calendar library was not useful to us so we had to spend some days reading all the documentation to understand how to manipulate it so it can be functional.
Thank you!
 Christos Lagos
Iliana Beikou
Spyros Tsamis
George Chatziadis
John Podogorianiotis
 
