/*
Composable para guardar el tema actual del board
*/
export const useCurrentTheme = () => {
  const currentTheme = useCookie('currentTheme', {
    default: () => "classicTheme",
    path: "/"
  });
  return { currentTheme };
}