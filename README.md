# Music-online-shop Web application
Developed in C# .NET Core and styled with Bootstrap,
this web application was created with the purpose of selling music products (CDs, vinyl, cassettes, etc.) and
to create a user-friendly interface through which customers can easily purchase what they desire.
![image](https://github.com/user-attachments/assets/403344d6-48d3-4f11-a455-cff606e71e6b)

# User Roles
## 1. Administrator:
* Controls all products and categories
* Approves or rejects new products
* Manages categories and can create new ones
* Can delete or modify products and comments
* Manages user roles
## 2. Collaborator:
* Adds new products (requires approval)
* Can modify their own products
* After modification, the product must be approved again
## 3.Registered User:
* Adds products to cart
* Can leave reviews and ratings
* Modifies their own reviews
* Views all products and comments
## 4.Unregistered User:
* Only views products and comments
* Is redirected to registration when wanting to purchase
# Main Functionalities:
* **Adding Products:**<br />
Collaborator and admin users can add products to the store. These products must be approved by the administrator before being visible. The administrator has the option to reject or approve these requests.
* **Category Management**:<br />
Products are organized in categories created dynamically by the administrator. They can add new categories directly from the application interface, as well as view, edit or delete existing categories.
* **Product Details**:<br />
Each product contains the following mandatory information: title, description, photo, price, stock and rating. The system includes a rating mechanism from 1 to 5 stars. Each user can give a single rating per product, and the final score is calculated based on all existing ratings.
* **Reviews**:<br />
Products can receive text reviews from users. The review includes a text comment left by the user. This field is optional, but the rest of the fields are mandatory. Stock represents the number of available products.
* **Product Editing**:<br />
The collaborator user can edit and delete products added by them. After editing, the product requires approval from the administrator again.
* **Product Search**:<br />
Products can be searched by name through a search engine. Also, products can be searched by name and must be found even if the user searches only certain parts that make up the name.
* **Sorting and Filtering**:<br />
Search engine results can be sorted ascending or descending based on price and number of stars. The system implements filters from which the user can choose the desired options.
* **Administration**:<br />
The administrator can delete and edit both products and comments. They can also activate or revoke user rights.
# Setup Instructions:
## Prerequisites:
* Visual Studio 2022 or later<br>
* .NET 6.0 SDK or later<br>
* SQL Server (Local or Express)<br>
## Database Setup:
> [!NOTE]
> Use these commands in terminal<br>
> Add-migration [migration_name]<br>
> Update-Database<br>
## Role Configuration:
### The application comes with the following predefined roles:<br>
* Administrator<br>
* Collaborator<br>
* UserN (Normal User)<br>
* UserI (Identified User)<br>
### Default admin credentials:<br>
* Email: admin@test.com<br>
* Password: Admin1!<br>
## Running the application:
* Open the solution in Visual Studio<br>
* Restore NuGet packages<br>
* Build the solution<br>
* Run the application<br>
# Examples (screenshots)

The admin can modify user roles:
![image](https://github.com/user-attachments/assets/de318914-90b0-46f9-a4ee-5fb123ce9f25)

The admin or collaborators can add new products to the web application database:![image](https://github.com/user-attachments/assets/8647feaa-7571-4575-b30a-7b5b9744ab27)
![image](https://github.com/user-attachments/assets/93b8bdf5-1222-4e6f-b614-0ba8fc24119e)

Only the admin can approve/reject them:![image](https://github.com/user-attachments/assets/69fd5a71-7e8c-44ad-8793-e4db208cf0ae)

After approval, the article will appear on the main page visible to all users:![image](https://github.com/user-attachments/assets/82d855d3-0a22-41fd-88de-0a814766bb3e)





 
