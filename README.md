# gitleaks-config-helper

Very simple C# (.Net 5) code that allows to merge two *.toml files into one.

This tool's goal is to allow enterprise-scale deployment of GitLeaks: https://github.com/zricethezav/gitleaks/

Every project is different and requires custom configuration. But you also want to have ability to fix issues or introduce new rules in central point

With this tool you can run following logic in your CI/CD pipeline 

1. Get base file shared with every project
2. Create local rules or override rules from base file (using same id)
3. Generate compiled final file for your project.

Or.

1. Create config file for each project type 
2. Merge several files into one depending on how your solution is constructed

Hope it will help somebody.

