/*
  This file is part of libmicrohttpd
  Copyright (C) 2015 Karlson2k (Evgeny Grin)

  This library is free software; you can redistribute it and/or
  modify it under the terms of the GNU Lesser General Public
  License as published by the Free Software Foundation; either
  version 2.1 of the License, or (at your option) any later version.

  This library is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
  Lesser General Public License for more details.

  You should have received a copy of the GNU Lesser General Public
  License along with this library; if not, write to the Free Software
  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

/**
 * @file microhttpd/mhd_str.c
 * @brief  Functions implementations for string manipulating
 * @author Karlson2k (Evgeny Grin)
 */

#include "mhd_str.h"

#include "MHD_config.h"

#ifdef HAVE_STDBOOL_H
#include <stdbool.h>
#endif

/*
 * Block of functions/macros that use US-ASCII charset as required by HTTP
 * standards. Not affected by current locale settings.
 */

#ifdef INLINE_FUNC
 /**
 * Check whether character is lower case letter in US-ASCII
 * @param c character to check
 * @return non-zero if character is lower case letter, zero otherwise
 */
_MHD_inline _MHD_bool
isasciilower (char c)
{
  return c >= 'a' && c <= 'z';
}

/**
 * Check whether character is upper case letter in US-ASCII
 * @param c character to check
 * @return non-zero if character is upper case letter, zero otherwise
 */
_MHD_inline _MHD_bool
isasciiupper (char c)
{
  return c >= 'A' && c <= 'Z';
}

/**
 * Check whether character is letter in US-ASCII
 * @param c character to check
 * @return non-zero if character is letter in US-ASCII, zero otherwise
 */
_MHD_inline _MHD_bool
isasciialpha (char c)
{
  return isasciilower (c) || isasciiupper (c);
}

/**
 * Check whether character is decimal digit in US-ASCII
 * @param c character to check
 * @return non-zero if character is decimal digit, zero otherwise
 */
_MHD_inline _MHD_bool
isasciidigit (char c)
{
  return c >= '0' && c <= '9';
}

/**
 * Check whether character is decimal digit or letter in US-ASCII
 * @param c character to check
 * @return non-zero if character is decimal digit or letter, zero otherwise
 */
_MHD_inline _MHD_bool
isasciialmun (char c)
{
  return isasciialpha (c) || isasciidigit (c);
}

/**
 * Convert US-ASCII character to lower case.
 * If character is upper case letter in US-ASCII than it's converted to lower
 * case analog. If character is NOT upper case letter than it's returned
 * unmodified.
 * @param c character to check
 * @return converted to lower case character
 */
_MHD_inline char
toasciilower (char c)
{
  return isasciiupper (c) ? (c - 'A' + 'a') : c;
}

 /**
 * Convert US-ASCII character to upper case.
 * If character is lower case letter in US-ASCII than it's converted to upper
 * case analog. If character is NOT lower case letter than it's returned
 * unmodified.
 * @param c character to check
 * @return converted to upper case character
 */
_MHD_inline char
toasciiupper (char c)
{
  return isasciilower (c) ? (c - 'a' + 'A') : c;
}

#else  /* !INLINE_FUNC */

/**
 * Checks whether character is lower case letter in US-ASCII
 * @param c character to check
 * @return boolean true if character is lower case letter,
 *         boolean false otherwise
 */
#define isasciilower(c) (((char)(c)) >= 'a' && ((char)(c)) <= 'z')

/**
 * Checks whether character is upper case letter in US-ASCII
 * @param c character to check
 * @return boolean true if character is upper case letter,
 *         boolean false otherwise
 */
#define isasciiupper(c) (((char)(c)) >= 'A' && ((char)(c)) <= 'Z')

/**
 * Checks whether character is letter in US-ASCII
 * @param c character to check
 * @return boolean true if character is letter, boolean false
 *         otherwise
 */
#define isasciialpha(c) (isasciilower(c) || isasciiupper(c))

/**
 * Check whether character is decimal digit in US-ASCII
 * @param c character to check
 * @return boolean true if character is decimal digit, boolean false
 *         otherwise
 */
#define isasciidigit(c) (((char)(c)) >= '0' && ((char)(c)) <= '9')

 /**
 * Check whether character is decimal digit or letter in US-ASCII
 * @param c character to check
 * @return boolean true if character is decimal digit or letter,
 *         boolean false otherwise
 */
#define isasciialmun(c) (isasciialpha(c) || isasciidigit(c))

/**
 * Convert US-ASCII character to lower case.
 * If character is upper case letter in US-ASCII than it's converted to lower
 * case analog. If character is NOT upper case letter than it's returned
 * unmodified.
 * @param c character to check
 * @return converted to lower case character
 */
#define toasciilower(c) ((isasciiupper(c)) ? (((char)(c)) - 'A' + 'a') : ((char)(c)))

/**
 * Convert US-ASCII character to upper case.
 * If character is lower case letter in US-ASCII than it's converted to upper
 * case analog. If character is NOT lower case letter than it's returned
 * unmodified.
 * @param c character to check
 * @return converted to upper case character
 */
#define toasciiupper(c) ((isasciilower(c)) ? (((char)(c)) - 'a' + 'A') : ((char)(c)))
#endif /* !INLINE_FUNC */

/**
 * Check two string for equality, ignoring case of US-ASCII letters.
 * @param str1 first string to compare
 * @param str2 second string to compare
 * @return non-zero if two strings are equal, zero otherwise.
 */
int
MHD_str_equal_caseless_ (const char * str1, const char * str2)
{
  while (0 != (*str1))
    {
      const char c1 = *str1;
      const char c2 = *str2;
      if (c1 != c2 && toasciilower (c1) != toasciilower (c2))
        return 0;
      str1++;
      str2++;
    }
  return 0 == (*str2);
}


/**
 * Check two string for equality, ignoring case of US-ASCII letters and
 * checking not more than @a maxlen characters.
 * Compares up to first terminating null character, but not more than
 * first @a maxlen characters.
 * @param str1 first string to compare
 * @param str2 second string to compare
 * @patam maxlen maximum number of characters to compare
 * @return non-zero if two strings are equal, zero otherwise.
 */
int
MHD_str_equal_caseless_n_ (const char * const str1, const char * const str2, size_t maxlen)
{
  for (size_t i = 0; i < maxlen; ++i)
    {
      const char c1 = str1[i];
      const char c2 = str2[i];
      if (0 == c2)
        return 0 == c1;
      if (c1 != c2 && toasciilower (c1) != toasciilower (c2))
        return 0;
    }
  return !0;
}
