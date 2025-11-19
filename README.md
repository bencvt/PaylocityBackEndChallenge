# Thanks in advance

Thanks in advance for reading these notes and the code I've written. I'm looking forward to discussing more!

# Design overview

Here's a high level overview of the changes I made to implement the requirements:

* Implemented the missing functionality for the `Employees` and `Dependents` controllers, including handling errors.

* Moved the hard-coded DTOs out of the controller, and into:
  * `Converter` extension classes to encapsulate going from models to DTOs.
  * `Retriever` classes that create *models*, not DTOs. In a production environment these classes would involve a SQL database. For this coding exercise the data remains hard-coded and synchronous, but decoupled from the controllers which is the important thing.
  * **Retrievers are single-use.** Each instance runs a single query and returns the model (or list of models). When subqueries are necessary, the retriever can instantiate its own sub-retrievers (e.g., `EmployeeRetriever` can create an arbitrary number of `DependentRetrievers`). The single-use design makes for more maintainable code. Lingering state can cause subtle bugs, so make it stateless!

* Added a `Paychecks` controller, working similar to the existing ones.
  * Includes integration tests.
  * For the purposes of this coding exercise, paychecks are only generated on the fly rather than pulled from a SQL database. The main class here is `PaycheckCalculator` rather than `PaycheckRetriever`. In an actual system, it would make sense to have both of those classes. The results of the calculator, when run by payroll rather than by an individual employee, would write to the database once the funds are sent. The retriever would then be used for past checks.

* The `Calculations` classes are where I spent the most time making sure to follow clean code guidelines:
  * The classes and methods are as small and clear as possible.
  * I tried to keep each set of calculations in its own class, so there are five classes instead of one. Aside from being easier to read, it makes unit testing a lot cleaner too.
  * Unit tests are decently thorough.
  * Like `Retriever` classes, each `Calculator` class is single-use, for the same reason as discussed above. (I left a couple as static classes because they are very simple; the extra boilerplate would have arguably hurt readability.)

* All of the values used in the calculations are configurable via `appsettings.json` and the `CalculatorConfiguration` class.
  * In an actual payroll system, these settings would have their own database storage and UI for payroll administrators to change as needed.

# Requirements and edge cases

Some of the requirements had a *lot* of ambiguity. In a real world project, I would work with the team to disambiguate each of these points rather than just making my own decision.

In any case, here are the most important bits:

 * **What parameters does the paycheck API need?** This wasn't specified but I decided to go with:
   * EmployeeId
   * A date, which is converted to the check date: the last day of the pay period containing the date. This check date is what I use for any date-related calculations (i.e., the additional $200 for dependents over age 50).

 * **What should the paycheck API call return?** This wasn't specified but I came up with:
   * Gross pay (annual salary / 26, rounded)
   * Net pay (gross minus deductions)
   * The check date (see above)
   * The current pay period (1 to 26, inclusive)
   * A list detailing each deductions. This wasn't mentioned in the requirements and returning it from the API required a lot more infrastructure, but it's well worth it in my opinion. It's in everybody's interest to be transparent about how everything is calculated, and it makes validation and troubleshooting much easier.

 * **What exactly does *"spread as evenly as possible on each paycheck"* mean, and what about rounding to the nearest cent?**
   * See `RoundingCalculator` and its unit test suite for the design I went with.
   * It's a small class but it's very important. There are more comments than code.

 * **How is the dependent type rule applied?** I opted for:
   * If an employee has an invalid set of dependents (e.g., multiple spouses), the `Employees` API still returns them all.
   * However the `Paychecks` API will error out, responding with an HTTP 404 instead of generating a paycheck.

# Thanks again!

Best,  
Ben Cartwright  
bencvt@gmail.com
