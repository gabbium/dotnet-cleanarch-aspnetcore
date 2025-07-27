import { execa } from "execa";

export async function publish(pluginConfig, context) {
  const { nextRelease } = context;

  if (!nextRelease) {
    console.log("⚠️ No nextRelease info, skipping NuGet publish.");
    return;
  }

  const version = nextRelease.version;
  console.log(`📦 Packing NuGet with version ${version}...`);

  await execa("dotnet", ["build", "--configuration", "Release"], {
    stdio: "inherit",
  });

  await execa(
    "dotnet",
    [
      "pack",
      "--configuration",
      "Release",
      "--no-build",
      "--no-restore",
      "--output",
      "./nupkgs",
      `/p:Version=${version}`,
    ],
    { stdio: "inherit" }
  );

  await execa(
    "dotnet",
    [
      "nuget",
      "push",
      "./nupkgs/*.nupkg",
      "-k",
      process.env.NUGET_TOKEN,
      "-s",
      "https://api.nuget.org/v3/index.json",
    ],
    { stdio: "inherit" }
  );

  console.log(`✅ NuGet ${version} published!`);
}

