describe('template spec', () => {
    it('Open Google', () => {
        cy.visit('https://www.google.com/')
        cy.get('#APjFqb').type('Arun Manglick {enter}')
        // cy.get(':nth-child(3) > .g > .N54PNb > .jGGQ5e > .yuRUbf > :nth-child(1) > [jscontroller="msmzHf"] > a > .LC20lb').click()
        // cy.get(':nth-child(3) > .g > .N54PNb > .jGGQ5e > .yuRUbf > :nth-child(1) > [jscontroller="msmzHf"] > a > .notranslate > .q0vns > .CA5RN > .byrV5b > .tjvcx').click()

        cy.wait(4000)
        cy.contains('Videos').click()
    })
  })