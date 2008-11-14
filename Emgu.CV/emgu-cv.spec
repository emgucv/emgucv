%define _unpackaged_files_terminate_build 0
Summary: A cross platform .NET wrapper for the Intel OpenCV image-processing library. Allows OpenCV functions to be called from .NET compatible languages such as C Sharp, VB, VC++, IronPython. The wrapper can be compiled in Mono and run on Linux & Mac OS X. 
Name: emgu-cv
Release: 1
License: GPL
Group: Emgu
URL: http://www.emgu.com/wiki 
Source0: %{name}-%{version}.tar.gz
BuildRoot: %{_topdir}/BUILD/%{name}-%{version}

%description

%prep
%setup -q

%build
make rpmbin 

%install

%clean
rm -rf $RPM_BUILD_ROOT

%files
%defattr(-,root,root,-)
%doc README.txt
/var/lib/emgu/bin/Emgu.CV.dll

%changelog
* Mon Dec 31 2007 Canming Huang <huangcanming@hotmail.com> - 
- Initial build.

%post
gacutil -i /var/lib/emgu/bin/Emgu.CV.dll

%preun
gacutil -u Emgu.CV



