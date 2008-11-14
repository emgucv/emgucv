%define _unpackaged_files_terminate_build 0
Summary: The emgu utility components
Name: emgu-util
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
/var/lib/emgu/bin/Emgu.Util.dll

%changelog
* Mon Dec 31 2007 Canming Huang <huangcanming@hotmail.com> - 
- Initial build.

%post
gacutil -i /var/lib/emgu/bin/Emgu.Util.dll

%preun
gacutil -u Emgu.Util



