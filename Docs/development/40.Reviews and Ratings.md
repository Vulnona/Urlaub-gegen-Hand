User Story: 
 After a successful agreement has been reached for the acceptance of an offer, a user can give a review to the host and vice versa. 

 Backend: 
Navigate to "Backend" folder, run the project file in Visual Studio 2022 and open "Reviews" Controller which handles the requests to Add, Update or Delete an offer. 

API Endpoints:
1. HttpPost("review/adding-review"): Responsible to add review on the user's profile
2. HttpPut("review/update-review-status") : To update the user review (If required)
3. HttpGet("review/get-all-reviews") : Get all reviews
4. HttpGet("review/get-user-by-offerId/{offerId}") :To get the user who posted the Offer
5. HttpGet("review/get-user-by-review-id/{reviewId}") : To get the user by review ID
6. HttpGet("review/check-review-status"): Check review status, if the review is published or not

Frontend: 

Navigate to the root directory of your project and open Frontend-Vuetify folder and open Visual studio code on the selected path: 

Install dependencies:

Project setup
-----------------------
While using "yarn"
yarn

if using "npm"
npm install

if using "pnpm"
pnpm install

if using bun 
bun install
------------------------

Compiling and hot-reloading for development

------------------------
While using  "yarn"
yarn dev

While using "npm"
npm run dev

While using "pnpm"
pnpm dev

While using "bun"
bun run dev
------------------------

Compiling and minifizing files for production

------------------------
While using "yarn"
yarn build

While using  "npm"
npm run build

While using "pnpm"
pnpm build

While using "bun"
bun run build
-------------------------

Lints and fixes files

--------------------------
While using "yarn"
yarn lint

While using "npm"
npm run lint

While using "pnpm"
pnpm lint

While using "bun"
bun run lint
-------------------------

PostReview.vue:
This page is responsibe to add review on the users profile. 
   
