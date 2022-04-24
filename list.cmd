git add . --dry-run
git status --ignored
echo **/bin/Debug/** > .gitignore
echo **/bin/Release/** >> .gitignore
echo **/obj/Debug/** >> .gitignore
echo **/obj/Release/** >> .gitignore
git add .gitignore & git commit -m "gitignore update"
pause