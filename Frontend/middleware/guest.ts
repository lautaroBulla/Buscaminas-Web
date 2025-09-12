/*
Middleware simplemente que se encarga de verificar si hay un usuario authenticado
asi evitar el acceso a paginas
*/

export default defineNuxtRouteMiddleware(async () => {
  const { user, isAuthReady, getProfile } = useAuth();
  
  if (!isAuthReady.value) {
    await getProfile();
  }

  if (user.value) {
    return navigateTo('/');
  }
});