#!/usr/bin/perl

# svn-clean - Wipes out unversioned files from SVN working copy.
# Copyright (C) 2004, 2005, 2006 Simon Perreault
#
# This program is free software; you can redistribute it and/or
# modify it under the terms of the GNU General Public License
# as published by the Free Software Foundation; either version 2
# of the License, or (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program; if not, write to the Free Software
# Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

use strict;
use Cwd;
use File::Path;
use Getopt::Long;
use Pod::Usage;

# Try to use SVN module if available.
my $use_svn_module = eval { require SVN::Client };

my $CWD = getcwd;

my @exclude      = ();
my $force        = 0;
my $quiet        = 0;
my $print        = 0;
my $help         = 0;
my $man          = 0;
my $nonrecursive = 0;
my $path         = $CWD;
GetOptions(
    "exclude=s"       => \@exclude,
    "force"           => \$force,
    "non-recursive|N" => \$nonrecursive,
    "quiet"           => \$quiet,
    "print"           => \$print,
    "help|?"          => \$help,
    "man"             => \$man
) or pod2usage(2);
pod2usage(1) if $help;
pod2usage( -exitstatus => 0, -verbose => 2 ) if $man;
$path = Cwd::abs_path( $ARGV[0] ) if @ARGV;

# Precompile regexes.
$_ = qr/$_/ foreach @exclude;

if ($use_svn_module) {

    # Create SVN client object. No need for connection to remote server.
    my $ctx = new SVN::Client;

    # Call handler function with status info for each file.
    $ctx->status( $path, undef, \&clean, !$nonrecursive, 1, 0, 1 );
}
else {
    warn "Warning: Not using SVN Perl modules, this might be slow.\n"
      unless $quiet;

    # Build svn client command
    my @command = qw(svn status --no-ignore -v);
    if ($nonrecursive) {
        push @command, '-N';
    }

    # Main file-wiping loop.
    if ( $^O eq 'MSWin32' ) {

        # Perl on Windows currently doesn't have list pipe opens.
        open SVN, join( ' ', @command, @ARGV ) . '|'
          or die "Can't call program \"svn\": $!\n";
    }
    else {
        open SVN, "-|", @command, @ARGV
          or die "Can't call program \"svn\": $!\n";
    }
  LINE: while (<SVN>) {
        if (/^([\?ID])/) {
            my $file = (split)[-1];
            foreach my $ex (@exclude) {
                if ( $file =~ $ex ) {
                    print "excluded $file\n" unless $quiet or $print;
                    next LINE;
                }
            }
            if ( $1 eq 'D' ) {
                next unless -f $file;
            }
            else {
                next unless -e $file;
            }
            if ($print) {
                print "$file\n";
            }
            else {
                rmtree( $file, !$quiet, !$force );
            }
        }
    }
}

# Main file-wiping function.
sub clean {
    my ( $path, $status ) = @_;

    # Use relative path for pretty-printing.
    if ( $path =~ s/^\Q$CWD\E\/?//o ) {

        # If the substitution succeeded, we should have a relative path. Make
        # sure we don't delete critical stuff.
        return if $path =~ /^\//;
    }

    # Find files needing to be removed.
    if (   $status->text_status == $SVN::Wc::Status::unversioned
        or $status->text_status == $SVN::Wc::Status::ignored
        or $status->text_status == $SVN::Wc::Status::deleted )
    {
        foreach my $ex (@exclude) {
            if ( $path =~ $ex ) {
                print "excluded $path\n" unless $quiet or $print;
                return;
            }
        }

        # Make sure the file exists before removing it. Do not remove deleted
        # directories as they are needed to remove the files they contain when
        # committing.
        lstat $path or stat $path;
        if (
            -e _
            and ( not -d _
                or $status->text_status != $SVN::Wc::Status::deleted )
          )
        {
            if ($print) {
                print "$path\n";
            }
            else {
                rmtree( $path, !$quiet, !$force );
            }
        }
    }
}

__END__

=head1 NAME

svn-clean - Wipes out unversioned files from Subversion working copy

=head1 SYNOPSIS

svn-clean [options] [directory or file ...]

=head1 DESCRIPTION

B<svn-clean> will scan the given files and directories recursively and find
unversioned files and directories (files and directories that are not present in
the Subversion repository). After the scan is done, these files and directories
will be deleted.

If no file or directory is given, B<svn-clean> defaults to the current directory
(".").

B<svn-clean> uses the SVN Perl modules if they are available. This is much
faster than parsing the output of the B<svn> command-line client.

=head1 OPTIONS

=over 8

=item B<-e>, B<--exclude>

A regular expression for filenames to be exluded. For example, the following
command will skip files ending in ".zip":

=over 8

svn-clean --exclude '\.zip$'

=back

Multiple exclude patterns can be specified. If at least one matches, then the
file is skipped. For example, the following command will skip files ending in
".jpg" or ".png":

=over 8

svn-clean --exclude '\.jpg$' --exclude '\.png$'

=back

The following command will skip the entire "build" subdirectory:

=over 8

svn-clean --exclude '^build(/|$)'

=back

=item B<-f>, B<--force>

Files to which you do not have delete access (if running under VMS) or write
access (if running under another OS) will not be deleted unless you use this
option.

=item B<-N>, B<--non-recursive>

Do not search recursively for unversioned files and directories. Unversioned
directories will still be deleted along with all their contents.

=item B<-q>, B<--quiet>

Do not print progress info. In particular, do not print a message each time a
file is examined, giving the name of the file, and indicating whether "rmdir" or
"unlink" is used to remove it, or that it's skipped.

=item B<-p>, B<--print>

Do not delete anything. Instead, print the name of every file and directory that
would have been deleted.

=item B<-?>, B<-h>, B<--help>

Prints a brief help message and exits.

=item B<--man>

Prints the manual page and exits.

=back

=head1 AUTHOR

Simon Perreault <nomis80@nomis80.org>

=cut
