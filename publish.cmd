@echo off
@msbuild build.msbuild /t:BuildPackageArtifacts /p:NugetSecret=<insert secret here>