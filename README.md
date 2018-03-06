# GD-Exercise
Green Dot Exercise
Given a list of packages determine the installation order by sorting the packagaes based on the dependency. All package dependencies should appear before the package itself.

Used depth first algorithm to solve this problem.
Select a package and recursively process and sort all its dependencies. Set the package "in process" state to true while the package is being processed. A cyclic reference is detected if a package "in process" is detected as a dependency.

Once the package is processes set the "in process" state to false and move though the list until all packages are processed.

