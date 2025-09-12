/*
los plugins .client se ejecutan del lado del cliente
este se encarga una vez que la aplicacion esta lista
hace getProfile de useAuth para obtener el perfil del usuario
onNuxtReady se asegura de que la aplicacion este lista antes de ejecutar el codigo
*/

export default defineNuxtPlugin(async (nuxtApp) => {
  const user = useState('user');

  if (user.value) return;

  onNuxtReady(async () => {
    const { getProfile } = useAuth();
    await getProfile();
  })
})
