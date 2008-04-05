%define _unpackaged_files_terminate_build 0
Summary: The emgu cv components
Name: emgu-cv
Release: 1
License: GPL
Group: Emgu
URL: www.emgu.com 
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
%doc README
/var/lib/emgu/bin/Emgu.CV.dll

%changelog
* Mon Dec 31 2007 Canming Huang <canming@devel.emgu.com> - 
- Initial build.

%post
gacutil -i /var/lib/emgu/bin/Emgu.CV.dll

%preun
gacutil -u Emgu.CV



