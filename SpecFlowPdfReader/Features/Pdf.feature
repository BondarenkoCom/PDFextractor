Feature: Extract PDF Text

@ScenarioDependency(0)
Scenario: ab text from a PDF file
Given my PDF file "Auto-Insurance-Verification-Letter.pdf"
When I extract the text

@ScenarioDependency(1)
Scenario: check title from a Json file
Then the Json should have a non-null document section title

@ScenarioDependency(2)
Scenario: check header from a Json file
Then the Json should have a non-null document section header

@ScenarioDependency(3)
Scenario: check paragraph from a PDF file
Then the Json should have a non-null document section paragraph

@ScenarioDependency(4)
Scenario: check box from a Json file
Then the Json should have a non-null document section checkbox

@ScenarioDependency(5)
Scenario: table from a Json file 
Then the Json should have a non-null document section table