# Contributing

This project follows **Conventional Commits** and uses **semantic-release** for fully automated versioning.

---

## 📝 Conventional Commits

We use **Conventional Commits** so semantic-release can automatically bump versions and publish to NuGet.

### ✅ Examples

- **feat:** adds a new feature → _minor version bump_

  ```
  feat: add Result.Map extension method
  ```

- **fix:** bug fix → _patch version bump_

  ```
  fix: correct ErrorType serialization issue
  ```

- **feat!:** or add `BREAKING CHANGE:` in the body → _major version bump_

  ```
  feat!: remove legacy CQRS interface

  BREAKING CHANGE: ICommand<T> was removed in favor of ICommandHandler<T>
  ```

### ❌ Avoid

- Messages like `update stuff` or `fix bug`
- Commits without context

---

## 🔄 What happens after merge?

When you merge into **main**:

- semantic-release will analyze commits
- Decide the next version (patch/minor/major)
- Update `CHANGELOG.md` automatically
- Create a GitHub Release

No manual versioning or tagging is needed!

---

## ✅ Commit Types

| Type     | When to use                                                |
| -------- | ---------------------------------------------------------- |
| feat     | A new feature                                              |
| fix      | A bug fix                                                  |
| docs     | Documentation only changes                                 |
| style    | Code style changes (formatting, missing semi-colons, etc.) |
| refactor | Code change that neither fixes a bug nor adds a feature    |
| test     | Adding or fixing tests                                     |
| chore    | Maintenance tasks (build, tooling, CI, etc.)               |
