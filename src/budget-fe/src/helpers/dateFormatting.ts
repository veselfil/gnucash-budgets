
const currencyFormatMap: { [key: string]: any } = {
  "EUR": Intl.NumberFormat("cs-CZ", { style: 'currency', currency: 'EUR' }),
  "USD": Intl.NumberFormat("cs-CZ", { style: 'currency', currency: 'USD' }),
  "CZK": Intl.NumberFormat("cs-CZ", { style: 'currency', currency: 'CZK' })
}

export function formatCurrency(value: number, currencyCode: string) {
  if (!Object.hasOwn(currencyFormatMap, currencyCode))
    return "UNKNOWN " + value;
  else
    return currencyFormatMap[currencyCode].format(value);
}