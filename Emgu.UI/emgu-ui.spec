%define _unpackaged_files_terminate_build 0
Summary: The emgu user interfaces
Name: emgu-ui
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
/var/lib/emgu/bin/Emgu.UI.dll

%changelog
* Mon Dec 31 2007 Canming Huang <canming@devel.emgu.com> - 
- Initial build.

%post
gacutil -i /var/lib/emgu/bin/Emgu.UI.dll

%preun
gacutil -u Emgu.UI



