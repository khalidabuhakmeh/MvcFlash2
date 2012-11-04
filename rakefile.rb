require 'albacore'

task :default => [:version, :build, :test]

desc "Version MvcFlash"
assemblyinfo :version do |asm|

	file = File.read("version.txt")
 
  	asm.version = file
  	asm.file_version = file

  	asm.company_name = "MvcFlash"
  	asm.product_name = "MvcFlash"
  	asm.copyright = "Khalid Abuhakmeh (c) 2012"
  	asm.output_file = "AssemblyInfo.cs"
  	asm.com_visible = false
  
 end

desc "Build MvcFlash"
msbuild :build => :version do |msb|

	msb.properties :configuration => :Release
	msb.targets :Clean, :Build
	msb.solution = "./MvcFlash.sln"

end

desc "Run unit tests"
xunit :test => :build do |xunit|
	xunit.command = "./packages/xunit.runners.1.9.1/tools/xunit.console.clr4.exe"
	xunit.assembly = "./MvcFlash.Tests/bin/Release/MvcFlash.Tests.dll"
end
