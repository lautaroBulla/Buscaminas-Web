<script setup>
  definePageMeta({
    middleware: ['guest']
  })

  import { z } from 'zod/v4';
  import MinePixelReveal from '~/components/MinePixelReveal.vue';
  import SHA256 from 'crypto-js/sha256';
  import Base64 from 'crypto-js/enc-base64';

  const { t } = useI18n();

  const { login } = useAuth();
  const { saveGame } = useGame();

  const schema = z.object({
    username: z.string().min(1, t('login.emptyUsername')),
    password: z.string().min(1, t('login.emptyPassword'))
  });

  const credentials = ref({
    username: '',
    password: ''
  });

  const errorMessage = ref('');
  const errors = ref({});

  const showPassword = ref(false);
  const sendingToBackend = ref(false);

  const submit = async () => {
    errorMessage.value = '';
    errors.value = {};

    const validation = schema.safeParse(credentials.value);
    if (!validation.success) {
      validation.error.issues.forEach(err => {
        const field = err.path[0];
        errors.value[field] = err.message;
      });
      return;
    }

    sendingToBackend.value = true;
    try {
      await login(credentials.value.username, credentials.value.password);

      const data = useCookie('gameToSave');
      const secret = useRuntimeConfig().public.secretHash;

      if (data && data.value) {
        const { hash, ...gameData } = data.value;
        const payload = JSON.stringify(gameData);
        const expectedHash = Base64.stringify(SHA256(payload + secret));

        if (hash === expectedHash) {
          await saveGame(
            gameData.help,
            gameData.seconds,
            gameData.difficulty,
            gameData.rows,
            gameData.cols,
            gameData.mines,
            gameData.n3BV,
            gameData.clicks,
            gameData.efficiency
          );
        }

        data.value = null;
      }
      sendingToBackend.value = false;
      return navigateTo('/');
    } catch (error) {
      sendingToBackend.value = false;
      let msg = error?.data?.message; 
      if (!msg) {
        errorMessage.value = t('login.failedLogin');
      } else {
        errorMessage.value = error?.data?.message;
      }
    }
  }

  const changePassword = () => {
    if (credentials.value.password) {
      const toggleShow = document.getElementById('toggleShow');
      toggleShow?.classList.remove('hidden');
    } else {
      const toggleShow = document.getElementById('toggleShow');
      toggleShow?.classList.add('hidden');
    }
  }
</script>

<template>
  <div class="flex items-center justify-center
              p-2
              md:pt-4"
  >
    <form @submit.prevent="submit" class="card flex flex-col items-center space-y-5 p-5 h-fit
                                          w-full 
                                          md:w-8/12 
                                          lg:w-6/12
                                          xl:w-4/12"
    >

        <p v-if="errorMessage">{{ errorMessage }}</p>

        <div class="flex flex-col items-start w-full">
          <p class="secondary">{{ $t('login.username') }}</p>
          <input
            type="text"
            class="input"
            v-model="credentials.username"
          />
          <p v-if="errors.username" class="text-sm">{{ errors.username }}</p>
        </div>

        <div class="flex flex-col items-start w-full">
          <p class="secondary">{{ $t('login.password') }}</p>
          <div class="relative w-full">
            <input
              :type="showPassword ? 'text' : 'password'"
              class="input w-full pr-10"
              v-model="credentials.password"
              @input="changePassword"
            />
            <button
              type="button"
              id="toggleShow"
              class="hidden absolute right-2 top-1/2 transform -translate-y-1/2 hover:cursor-pointer hover:opacity-80"
              @click="showPassword = !showPassword"
            >
              {{ showPassword ? $t('login.hide') : $t('login.show') }}
            </button>
          </div>
          <p v-if="errors.password" class="text-sm">{{ errors.password }}</p>
        </div>

        <div class="flex flex-col items-center w-full">
          <button v-if="!sendingToBackend" type="submit" class="primary">
            {{ $t('login.submit') }}
          </button>
          <MinePixelReveal
            v-else
            :width="'3px'"
            :height="'3px'"
          />
        </div>

    </form>
  </div>
</template>
