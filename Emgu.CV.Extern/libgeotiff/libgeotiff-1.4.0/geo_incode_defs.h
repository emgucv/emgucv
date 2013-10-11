
/*
 * This file is included by the CSV ".c" files in the csv directory.
 *
 * copyright (c) 1995   Niles D. Ritter
 *
 * Permission granted to use this software, so long as this copyright
 * notice accompanies any products derived therefrom.
 */

#include <stdio.h>

/* Data structures */
typedef const char * datafile_rows_t;
typedef struct datafile_s {
  const char *name;
  const datafile_rows_t **rows; 
} datafile_t;
