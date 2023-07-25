read -p "Enter database path [../../../testfiles/gnucash_sqlite.budgets]: " db_path
if [ -z "$db_path" ]; then
  db_path="../../../testfiles/gnucash_sqlite.budgets"
fi

read -p "Enter project path [./backend/GnuCashBudget.Data.EntityFramework]: " project_path
if [ -z "$project_path" ]; then
  project_path="./backend/GnuCashBudget.Data.EntityFramework"
fi

read -p "Enter migration name: " migration_name

cat "${db_path}"


(
  cd "${project_path}" || exit
  dotnet ef migrations add "${migration_name}"
  dotnet ef database update --connection "Data Source=${db_path}"
)