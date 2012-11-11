require 'albacore'

task :default => [:init, :version, :build, :test, :core, :web]

desc "initalize" 
task :init do |init| 
	if !FileTest::directory?("./build")
		Dir.mkdir("./build")
	else
		puts 'build directory already exists'
	end

	@version = file = File.read("version.txt")

end

desc "Version MvcFlash"
assemblyinfo :version => :init do |asm|

  	asm.version = @version
  	asm.file_version = @version

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
	xunit.html_output = "./build/"
end

desc "Create the nuget packages for: MvcFlash.Core"
nugetpack :core => :test do |nuget|
	update_version("mvcflash.core.nuspec")

	nuget.command = "./.nuget/nuget.exe"
	nuget.nuspec = "mvcflash.core.nuspec"
	nuget.output = "build/"
end

desc "Create the nuget packages for: MvcFlash.Web"
nugetpack :web => :core do |nuget|
	update_version("mvcflash.web.nuspec")

	nuget.command = "./.nuget/nuget.exe"
	nuget.nuspec = "mvcflash.web.nuspec"
	nuget.output = "build/"
end

def update_version(file_path)
	# modify the version number in the nuspec
	initial = File.read(file_path)
	doc = REXML::Document.new(initial)
	doc.context[:attribute_quote] = :quote

    doc.elements.each("//version") do |p|
        p.text = @version
    end

	# save the new versioned nuspec
	File.open(file_path , 'w') {|f| doc.write(f) }
end