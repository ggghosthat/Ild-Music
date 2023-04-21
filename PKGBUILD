# This is an example PKGBUILD file. Use this as a start to creating your own,
# and remove these comments. For more information, see 'man PKGBUILD'.
# NOTE: Please fill out the license field for your package! If it is unknown,
# then please put 'unknown'.

# Maintainer: Name <yourmail@mail.com>
pkgname="Ild-Music"
pkgver=0.1
pkgrel=1
epoch=
pkgdesc="Simple mp3 music player"
arch=(x86_64)
url="https://github.com/ggghosthat/Ild-Music"
license=('MIT')
groups=()
depends=()
makedepends=()
checkdepends=()
optdepends=()
provides=()
conflicts=()
replaces=()
backup=()
options=()
install=
changelog=
source=("git+$url")
noextract=()
md5sums=()
validpgpkeys=()


build() {
	#cd "$pkgname"
	# ./configure --prefix=/usr
	make
}

package() {
	cp "$pkgname/bin/Release/net6.0/arch-x64/Ild-Music alpha release arch-x64" /opt
	# make DESTDIR="$pkgdir/" install
}
