# balancereporter

## Assumptions
- There aren't any limit defined for balance / transaction, hopefully it fits to float boundaries
- Date and times for transactions are all in local time
- In the sample request json, there is a possibility to provide more than one account.
  - If that's the case, should the result summarized between accounts?
    - If yes, then currency does matter, so some sort of currency converter is needed
    - If not, then there should be a report / account result
  - If not, the incoming data scheme isn't correct.
  - Right now, it only allows 1 accounts to process
- I didn't do any data validation, I assumed the incoming data is valid domain-wise.
- I thought the result should not contain the request date's balance, because its not closed yet.
- I only counted the transactions in Booked state, but I thought there are more states.
