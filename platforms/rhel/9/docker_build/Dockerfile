FROM emgu/emgucv-ubi:ubi9-dotnet8

RUN git clone https://github.com/emgucv/emgucv.git
RUN cd emgucv; git submodule update --init --recursive
 
RUN cd emgucv;git pull

RUN cd emgucv/platforms/rhel/9;./cmake_configure

